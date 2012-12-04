namespace StealFocus.Forecast.WindowsAzure.HostedService
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Reflection;

    using log4net;

    using StealFocus.AzureExtensions.HostedService;

    internal class DeploymentDeleteForecastWorker : HostedServiceForecastWorker
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object SyncRoot = new object();

        private readonly IDeployment deployment;

        private readonly IOperation operation;

        private readonly Guid subscriptionId;

        private readonly string certificateThumbprint;

        private readonly string serviceName;

        private readonly string deploymentSlot;

        private readonly ScheduleDay[] scheduleDays;

        private readonly int pollingIntervalInMinutes;

        private DateTime lastTimeWeDidWork = DateTime.MinValue;

        public DeploymentDeleteForecastWorker(
            IDeployment deployment, 
            IOperation operation, 
            Guid subscriptionId, 
            string certificateThumbprint, 
            string serviceName,
            string deploymentSlot,
            ScheduleDay[] scheduleDays,
            int pollingIntervalInMinutes)
            : base(GetWorkerId(serviceName, deploymentSlot))
        {
            this.deployment = deployment;
            this.operation = operation;
            this.subscriptionId = subscriptionId;
            this.certificateThumbprint = certificateThumbprint;
            this.serviceName = serviceName;
            this.deploymentSlot = deploymentSlot;
            this.scheduleDays = scheduleDays;
            this.pollingIntervalInMinutes = pollingIntervalInMinutes;
        }

        public override void DoWork()
        {
            if (this.lastTimeWeDidWork.AddMinutes(this.pollingIntervalInMinutes) < DateTime.Now)
            {
                string doingWorkLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is doing work.", this.GetType().Name, this.Id);
                Logger.Debug(doingWorkLogMessage);
                bool nowIsInTheSchedule = DetermineIfNowIsInTheSchedule(Logger, this.GetType().Name, this.scheduleDays);
                if (nowIsInTheSchedule)
                {
                    lock (SyncRoot)
                    {
                        string checkingDeploymentExistsMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is checking if deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' exists.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                        Logger.Info(checkingDeploymentExistsMessage);

                        // Assume the deployment does not exist - we don't want to try and delete it until we confirm it exists.
                        bool deploymentExists = false;
                        try
                        {
                            deploymentExists = this.deployment.CheckExists(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                        }
                        catch (WebException e)
                        {
                            string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' experienced an error checking for deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'. The operation will be retried after the next polling interval.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Error(errorMessage, e);
                        }

                        if (deploymentExists)
                        {
                            string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is deleting deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' as it was found to exist.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Info(deleteDeploymentLogMessage);
                            try
                            {
                                string deleteRequestId = this.deployment.DeleteRequest(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                                this.WaitForResultOfRequest(Logger, this.GetType().Name, this.operation, this.subscriptionId, this.certificateThumbprint, deleteRequestId);
                            }
                            catch (WebException e)
                            {
                                string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' experienced an error deleting deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'. The operation will be retried after the next polling interval.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                                Logger.Error(errorMessage, e);
                            }
                        }
                        else
                        {
                            string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is not deleting deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' as it was not found to exist.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Info(deleteDeploymentLogMessage);
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

        private static string GetWorkerId(string serviceName, string deploymentSlot)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-{1}", serviceName, deploymentSlot);
        }
    }
}
