namespace StealFocus.Forecast.WindowsAzure.HostedService
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using log4net;

    using StealFocus.AzureExtensions;
    using StealFocus.AzureExtensions.HostedService;
    using StealFocus.Forecast.Configuration.WindowsAzure;

    internal class WhiteListForecastWorker : HostedServiceForecastWorker
    {
        private const int OneMinuteInMilliseconds = 60000;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object SyncRoot = new object();

        private readonly ISubscription[] subscriptions;

        private readonly IDeployment deployment;

        private readonly IOperation operation;

        private readonly string[] allowedServiceNames;

        public WhiteListForecastWorker(
            ISubscription[] subscriptions,
            IDeployment deployment,
            IOperation operation,
            string[] allowedServiceNames,
            int pollingIntervalInMinutes)
            : base(GetWorkerId(typeof(WhiteListForecastWorker).FullName), pollingIntervalInMinutes * OneMinuteInMilliseconds)
        {
            this.subscriptions = subscriptions;
            this.deployment = deployment;
            this.operation = operation;
            this.allowedServiceNames = allowedServiceNames;
        }

        public override void DoWork()
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
                        bool allowed = this.allowedServiceNames.Any(allowedServiceName => allowedServiceName == hostedServiceName);
                        if (!allowed)
                        {
                            this.DeleteDeployedService(subscription, hostedServiceName, DeploymentSlot.Staging);
                            this.DeleteDeployedService(subscription, hostedServiceName, DeploymentSlot.Production);
                        }
                    }
                }
            }
        }

        private static string GetWorkerId(string serviceName)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}", serviceName);
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
    }
}
