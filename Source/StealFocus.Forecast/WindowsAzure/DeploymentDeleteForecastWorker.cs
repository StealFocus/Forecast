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
            string doingWorkLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' is doing work.", this.Id);
            logger.Info(doingWorkLogMessage);
            DateTime timeOfPlannedDelete = DateTime.UtcNow;
            bool previousDeleteDayIsNotTheSameAsThePlannedDelete = this.PreviousDeleteDayIsNotTheSameAsThePlannedDelete(this.previousDelete, timeOfPlannedDelete);
            bool timeOfPlannedDeleteIsAfterScheduledTime = this.TimeOfPlannedDeleteIsAfterScheduledTime(this.scheduledTime, timeOfPlannedDelete);
            if (previousDeleteDayIsNotTheSameAsThePlannedDelete && timeOfPlannedDeleteIsAfterScheduledTime)
            {
                string checkingDeploymentExistsMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' is checking if deployment for Subscription ID '{1}', Service Name '{2}' and Deployment Slot '{3}' exists.", this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                logger.Info(checkingDeploymentExistsMessage);
                bool deploymentExists = this.deployment.CheckExists(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                if (deploymentExists)
                {
                    string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' is deleting deployment for Subscription ID '{1}', Service Name '{2}' and Deployment Slot '{3}' as it was found to exist.", this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                    logger.Info(deleteDeploymentLogMessage);
                    this.deployment.DeleteRequest(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                    this.previousDelete = timeOfPlannedDelete;
                }
                else
                {
                    string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' is not deleting deployment for Subscription ID '{1}', Service Name '{2}' and Deployment Slot '{3}' as it was not found to exist.", this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                    logger.Info(deleteDeploymentLogMessage);
                }
            }
        }

        private bool PreviousDeleteDayIsNotTheSameAsThePlannedDelete(DateTime timeOfPreviousDelete, DateTime timeOfPlannedDelete)
        {
            if (timeOfPreviousDelete.DayOfYear != timeOfPlannedDelete.DayOfYear)
            {
                string trueLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' has found that the previous delete day is not equal to the planned delete day.", this.Id);
                logger.Info(trueLogMessage);
                return true;
            }

            string falseLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' has found that the previous delete day is equal to the planned delete day.", this.Id);
            logger.Info(falseLogMessage);
            return false;
        }

        private bool TimeOfPlannedDeleteIsAfterScheduledTime(DateTime scheduledDeleteTime, DateTime timeOfPlannedDelete)
        {
            if (scheduledDeleteTime.Hour <= timeOfPlannedDelete.Hour && scheduledDeleteTime.Minute <= timeOfPlannedDelete.Minute)
            {
                string trueLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' has found that the schedule delete time is less than the planned delete time.", this.Id);
                logger.Info(trueLogMessage);
                return true;
            }

            string falseLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' has found that the schedule delete time is greater than or equal to the planned delete time.", this.Id);
            logger.Info(falseLogMessage);
            return false;
        }
    }
}
