namespace StealFocus.Forecast.WindowsAzure.HostedService
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Reflection;

    using log4net;

    using StealFocus.AzureExtensions.HostedService;

    internal class DeploymentCreateForecastWorker : ForecastWorker
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

        private readonly string deploymentName;

        private readonly Uri packageUrl;

        private readonly string label;

        private readonly string configurationFilePath;

        private readonly bool startDeployment;

        private readonly bool treatWarningsAsError;

        private readonly int pollingIntervalInMinutes;

        private DateTime lastTimeWeDidWork = DateTime.MinValue;

        public DeploymentCreateForecastWorker(
            IDeployment deployment,
            IOperation operation,
            Guid subscriptionId,
            string certificateThumbprint, 
            string serviceName,
            string deploymentSlot,
            ScheduleDay[] scheduleDays,
            string deploymentName,
            Uri packageUrl,
            string label,
            string configurationFilePath,
            bool startDeployment,
            bool treatWarningsAsError,
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
            this.deploymentName = deploymentName;
            this.packageUrl = packageUrl;
            this.label = label;
            this.configurationFilePath = configurationFilePath;
            this.startDeployment = startDeployment;
            this.treatWarningsAsError = treatWarningsAsError;
            this.pollingIntervalInMinutes = pollingIntervalInMinutes;
        }

        public override void DoWork()
        {
            if (this.lastTimeWeDidWork.AddMinutes(this.pollingIntervalInMinutes) < DateTime.Now)
            {
                string doingWorkLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is doing work.", this.GetType().Name, this.Id);
                Logger.Debug(doingWorkLogMessage);
                bool nowIsInTheSchedule = DetermineIfNowIsInTheSchedule(Logger, this.GetType().Name, this.Id, this.scheduleDays);
                if (nowIsInTheSchedule)
                {
                    lock (SyncRoot)
                    {
                        string checkingDeploymentExistsMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is checking if deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' exists.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                        Logger.Info(checkingDeploymentExistsMessage);

                        // Assume the deployment exists - we don't want to try and create it until we confirm it doesn't exist.
                        bool deploymentExists = true;
                        try
                        {
                            deploymentExists = this.deployment.CheckExists(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                        }
                        catch (WebException e)
                        {
                            string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' experienced an error checking for deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'. The operation will be retried after the next polling interval.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Error(errorMessage, e);
                        }

                        if (!deploymentExists)
                        {
                            string createDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is creating deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' as it was not found to exist.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Info(createDeploymentLogMessage);
                            try
                            {
                                string createRequestId = this.deployment.CreateRequest(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot, this.deploymentName, this.packageUrl, this.label, this.configurationFilePath, this.startDeployment, this.treatWarningsAsError);
                                ForecastWorker.WaitForResultOfRequest(Logger, this.GetType().Name, this.Id, this.operation, this.subscriptionId, this.certificateThumbprint, createRequestId);
                            }
                            catch (WebException e)
                            {
                                string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' experienced an error creating deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'. The operation will be retried after the next polling interval.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                                Logger.Error(errorMessage, e);
                            }
                        }
                        else
                        {
                            string createDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is not creating deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' as it was found to already exist.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Info(createDeploymentLogMessage);
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
