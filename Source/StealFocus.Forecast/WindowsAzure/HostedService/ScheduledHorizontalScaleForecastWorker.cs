namespace StealFocus.Forecast.WindowsAzure.HostedService
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using log4net;

    using StealFocus.AzureExtensions.HostedService;

    internal class ScheduledHorizontalScaleForecastWorker : HostedServiceForecastWorker
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object SyncRoot = new object();

        private readonly IDeployment deployment;

        private readonly IOperation operation;

        private readonly Guid subscriptionId;

        private readonly string certificateThumbprint;

        private readonly string serviceName;

        private readonly string deploymentSlot;

        private readonly HorizontalScale[] horizontalScales;

        private readonly ScheduleDay[] scheduleDays;

        private readonly bool treatWarningsAsError;

        private readonly string mode;

        private readonly int pollingIntervalInMinutes;

        private DateTime lastTimeWeDidWork = DateTime.MinValue;

        public ScheduledHorizontalScaleForecastWorker(
            IDeployment deployment,
            IOperation operation,
            Guid subscriptionId,
            string certificateThumbprint, 
            string serviceName,
            string deploymentSlot,
            HorizontalScale[] horizontalScales,
            ScheduleDay[] scheduleDays,
            bool treatWarningsAsError,
            string mode,
            int pollingIntervalInMinutes)
            : base(GetWorkerId(serviceName, deploymentSlot))
        {
            this.deployment = deployment;
            this.operation = operation;
            this.subscriptionId = subscriptionId;
            this.certificateThumbprint = certificateThumbprint;
            this.serviceName = serviceName;
            this.deploymentSlot = deploymentSlot;
            this.horizontalScales = horizontalScales;
            this.scheduleDays = scheduleDays;
            this.treatWarningsAsError = treatWarningsAsError;
            this.mode = mode;
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
                        bool deploymentExists = false;
                        try
                        {
                            deploymentExists = this.deployment.CheckExists(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                        }
                        catch (Exception e)
                        {
                            string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' experienced an error checking for deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'. The operation will be retried after the next polling interval.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Error(errorMessage, e);
                        }

                        if (deploymentExists)
                        {
                            string checkingScalingLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is checking the scaling for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Info(checkingScalingLogMessage);
                            try
                            {
                                string horizontallyScaleRequestId = this.deployment.HorizontallyScale(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot, this.horizontalScales, this.treatWarningsAsError, this.mode);
                                if (string.IsNullOrEmpty(horizontallyScaleRequestId))
                                {
                                    string scalingNotRequiredLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' did not scale for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' as it was found to already be scaled to the specification.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                                    Logger.Info(scalingNotRequiredLogMessage);
                                }
                                else
                                {
                                    string scalingRequiredLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' scaled for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                                    Logger.Info(scalingRequiredLogMessage);
                                    this.WaitForResultOfRequest(Logger, this.GetType().Name, this.operation, this.subscriptionId, this.certificateThumbprint, horizontallyScaleRequestId);
                                    string scalingSuccessLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' successfully scaled Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                                    Logger.Info(scalingSuccessLogMessage);
                                }
                            }
                            catch (Exception e)
                            {
                                string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' experienced an error horizontally scaling deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'. The operation will be retried after the next polling interval.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                                Logger.Error(errorMessage, e);
                            }
                        }
                        else
                        {
                            string scaleDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is not horizontally scaling deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' as it was not found to exist.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Warn(scaleDeploymentLogMessage);
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
