namespace StealFocus.Forecast.WindowsAzure.HostedService
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using log4net;

    using StealFocus.AzureExtensions;
    using StealFocus.AzureExtensions.HostedService;

    using StealFocus.Forecast.Configuration;
    using StealFocus.Forecast.Configuration.WindowsAzure;
    using StealFocus.Forecast.Configuration.WindowsAzure.HostedService;

    internal class WhiteListForecastWorker : HostedServiceForecastWorker
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object SyncRoot = new object();

        private readonly ISubscription[] subscriptions;

        private readonly IDeployment deployment;

        private readonly IOperation operation;

        private readonly WhiteListService[] allowedServices;

        private readonly int pollingIntervalInMinutes;

        private DateTime lastTimeWeDidWork = DateTime.MinValue;

        public WhiteListForecastWorker(
            ISubscription[] subscriptions,
            IDeployment deployment,
            IOperation operation,
            WhiteListService[] allowedServices,
            int pollingIntervalInMinutes)
            : base(GetWorkerId(typeof(WhiteListForecastWorker).FullName))
        {
            this.subscriptions = subscriptions;
            this.deployment = deployment;
            this.operation = operation;
            this.allowedServices = allowedServices;
            this.pollingIntervalInMinutes = pollingIntervalInMinutes;
        }

        public override void DoWork()
        {
            if (this.lastTimeWeDidWork.AddMinutes(this.pollingIntervalInMinutes) < DateTime.Now)
            {
                string doingWorkLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is doing work.", this.GetType().Name, this.Id);
                Logger.Debug(doingWorkLogMessage);
                lock (SyncRoot)
                {
                    foreach (ISubscription subscription in this.subscriptions)
                    {
                        string[] hostedServiceNames = subscription.ListHostedServices();
                        foreach (string hostedServiceName in hostedServiceNames)
                        {
                            bool allowed = this.allowedServices.Any(allowedService => allowedService.Name == hostedServiceName);
                            if (allowed)
                            {
                                WhiteListService whiteListService = this.allowedServices.Single(allowedService => allowedService.Name == hostedServiceName);
                                this.CheckDeploymentAgainstItsWhiteListEntry(subscription, hostedServiceName, whiteListService, DeploymentSlot.Staging);
                                this.CheckDeploymentAgainstItsWhiteListEntry(subscription, hostedServiceName, whiteListService, DeploymentSlot.Production);
                            }
                            else
                            {
                                this.DeleteDeployedService(subscription, hostedServiceName, DeploymentSlot.Staging);
                                this.DeleteDeployedService(subscription, hostedServiceName, DeploymentSlot.Production);
                            }
                        }
                    }
                }

                this.lastTimeWeDidWork = DateTime.Now;
            }
            else
            {
                string notDoingWorkLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is skipping doing work as we did work within the current polling window.", this.GetType().Name, this.Id);
                Logger.Debug(notDoingWorkLogMessage);
            }
        }

        private static string GetWorkerId(string serviceName)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}", serviceName);
        }

        private void CheckDeploymentAgainstItsWhiteListEntry(ISubscription subscription, string hostedServiceName, WhiteListService whiteListService, string deploymentSlot)
        {
            bool deployedInstanceSizesExceedWhiteListConfiguration = this.FindIfDeployedInstanceSizesExceedWhiteListConfiguration(subscription, hostedServiceName, whiteListService, deploymentSlot);
            if (deployedInstanceSizesExceedWhiteListConfiguration)
            {
                this.DeleteDeployedService(subscription, hostedServiceName, deploymentSlot);
                return;
            }

            HorizontalScale[] horizontalScales = this.FindHorizontalScaleChangesComparedToWhitelist(subscription, hostedServiceName, whiteListService, deploymentSlot);
            if (horizontalScales.Length > 0)
            {
                this.HorizontallyScaleDeployedServiceRole(subscription, hostedServiceName, deploymentSlot, horizontalScales);
            }
        }

        private bool FindIfDeployedInstanceSizesExceedWhiteListConfiguration(ISubscription subscription, string hostedServiceName, WhiteListService whiteListService, string deploymentSlot)
        {
            foreach (WhiteListRole whiteListRole in whiteListService.Roles)
            {
                if (whiteListRole.MaxInstanceSize.HasValue)
                {
                    string actualInstanceSizeValue = this.deployment.GetInstanceSize(subscription.SubscriptionId, subscription.CertificateThumbprint, hostedServiceName, deploymentSlot, whiteListRole.Name);
                    InstanceSize actualInstanceSize = (InstanceSize)Enum.Parse(typeof(InstanceSize), actualInstanceSizeValue);
                    if (actualInstanceSize > whiteListRole.MaxInstanceSize.Value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private HorizontalScale[] FindHorizontalScaleChangesComparedToWhitelist(ISubscription subscription, string hostedServiceName, WhiteListService whiteListService, string deploymentSlot)
        {
            List<HorizontalScale> horizontalScales = new List<HorizontalScale>();
            foreach (WhiteListRole whiteListRole in whiteListService.Roles)
            {
                if (whiteListRole.MaxInstanceCount.HasValue)
                {
                    int instanceCount = this.deployment.GetInstanceCount(subscription.SubscriptionId, subscription.CertificateThumbprint, hostedServiceName, deploymentSlot, whiteListRole.Name);
                    if (instanceCount > whiteListRole.MaxInstanceCount)
                    {
                        HorizontalScale horizontalScale = new HorizontalScale { InstanceCount = whiteListRole.MaxInstanceCount.Value, RoleName = whiteListRole.Name };
                        horizontalScales.Add(horizontalScale);
                    }   
                }
            }

            return horizontalScales.ToArray();
        }

        private void DeleteDeployedService(ISubscription subscription, string hostedServiceName, string deploymentSlot)
        {
            try
            {
                bool exists = this.deployment.CheckExists(subscription.SubscriptionId, subscription.CertificateThumbprint, hostedServiceName, deploymentSlot);
                if (exists)
                {
                    string logMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' found a '{2}' deployment for service name '{3}' in subscription '{4}' and the service was not in the white list, the deployed service is being deleted.", this.GetType().FullName, this.Id, deploymentSlot, hostedServiceName, subscription.SubscriptionId);
                    Logger.Warn(logMessage);
                    string deleteRequestId = this.deployment.DeleteRequest(subscription.SubscriptionId, subscription.CertificateThumbprint, hostedServiceName, deploymentSlot);
                    this.WaitForResultOfRequest(Logger, this.GetType().FullName, this.operation, subscription.SubscriptionId, subscription.CertificateThumbprint, deleteRequestId);
                }
                else
                {
                    string logMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' found service name '{2}' in subscription '{3}' and the service was not in the white list, but there was no deployment in '{4}'.", this.GetType().FullName, this.Id, hostedServiceName, subscription.SubscriptionId, deploymentSlot);
                    Logger.Info(logMessage);
                }
            }
            catch (Exception e)
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' experienced an error deleting a deployment Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'. The operation will be retried after the next polling interval.", this.GetType().Name, this.Id, subscription.SubscriptionId, hostedServiceName, deploymentSlot);
                Logger.Error(errorMessage, e);
            }
        }

        private void HorizontallyScaleDeployedServiceRole(ISubscription subscription, string hostedServiceName, string deploymentSlot, HorizontalScale[] horizontalScales)
        {
            try
            {
                bool exists = this.deployment.CheckExists(subscription.SubscriptionId, subscription.CertificateThumbprint, hostedServiceName, deploymentSlot);
                if (exists)
                {
                    string logMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' found a '{2}' deployment for service name '{3}' in subscription '{4}' and the service was not in the white list, the deployed service is being horizontally scaled.", this.GetType().FullName, this.Id, deploymentSlot, hostedServiceName, subscription.SubscriptionId);
                    Logger.Warn(logMessage);
                    string scaleRequestId = this.deployment.HorizontallyScale(subscription.SubscriptionId, subscription.CertificateThumbprint, hostedServiceName, deploymentSlot, horizontalScales, true, Mode.Auto);
                    this.WaitForResultOfRequest(Logger, this.GetType().FullName, this.operation, subscription.SubscriptionId, subscription.CertificateThumbprint, scaleRequestId);
                }
                else
                {
                    string logMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' found service name '{2}' in subscription '{3}' and the service was not in the white list, but there was no deployment in '{4}'.", this.GetType().FullName, this.Id, hostedServiceName, subscription.SubscriptionId, deploymentSlot);
                    Logger.Info(logMessage);
                }
            }
            catch (Exception e)
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' experienced an error horizontally scaling a deployment Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'. The operation will be retried after the next polling interval.", this.GetType().Name, this.Id, subscription.SubscriptionId, hostedServiceName, deploymentSlot);
                Logger.Error(errorMessage, e);
            }
        }
    }
}
