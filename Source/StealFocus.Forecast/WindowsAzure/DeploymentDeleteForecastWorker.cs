namespace StealFocus.Forecast.WindowsAzure
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using log4net;

    public class DeploymentDeleteForecastWorker : ForecastWorker
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDeployment deployment;

        private readonly Guid subscriptionId;

        private readonly string certificateThumbprint;

        private readonly string serviceName;

        private readonly string deploymentSlot;

        private readonly DateTime scheduledTime;

        private DateTime previousDelete = DateTime.MinValue;

        public DeploymentDeleteForecastWorker(IDeployment deployment, Guid subscriptionId, string certificateThumbprint, string serviceName, string deploymentSlot, DateTime scheduledTime)
        {
            this.deployment = deployment;
            this.subscriptionId = subscriptionId;
            this.certificateThumbprint = certificateThumbprint;
            this.serviceName = serviceName;
            this.deploymentSlot = deploymentSlot;
            this.scheduledTime = scheduledTime;
        }

        protected override void DoWork()
        {
            DateTime timeOfPlannedDelete = DateTime.UtcNow;
            bool previousDeleteDayIsNotTheSameAsThePlannedDelete = PreviousDeleteDayIsNotTheSameAsThePlannedDelete(this.previousDelete, timeOfPlannedDelete);
            bool timeOfPlannedDeleteIsAfterScheduledTime = TimeOfPlannedDeleteIsAfterScheduledTime(this.scheduledTime, timeOfPlannedDelete);
            if (previousDeleteDayIsNotTheSameAsThePlannedDelete && timeOfPlannedDeleteIsAfterScheduledTime)
            {
                bool deploymentExists = this.deployment.CheckExists(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                if (deploymentExists)
                {
                    string logMessage = string.Format(CultureInfo.CurrentCulture, "Deleting deployment for Subscription ID '{0}', Service Name '{1}' and Deployment Slot '{2}'.", this.subscriptionId, this.serviceName, this.deploymentSlot);
                    logger.Info(logMessage);
                    this.deployment.DeleteRequest(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                    this.previousDelete = DateTime.UtcNow;
                }
            }
        }

        private static bool PreviousDeleteDayIsNotTheSameAsThePlannedDelete(DateTime timeOfPreviousDelete, DateTime timeOfPlannedDelete)
        {
            if (timeOfPreviousDelete.DayOfYear != timeOfPlannedDelete.DayOfYear)
            {
                return true;
            }

            return false;
        }

        private static bool TimeOfPlannedDeleteIsAfterScheduledTime(DateTime scheduledTime, DateTime timeOfPlannedDelete)
        {
            if (scheduledTime.Hour < timeOfPlannedDelete.Hour && scheduledTime.Minute < timeOfPlannedDelete.Minute)
            {
                return true;
            }

            return false;
        }
    }
}
