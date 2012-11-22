namespace StealFocus.Forecast.WindowsAzure
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;
    using log4net;

    public class DeploymentDeleteForecastWorker : ForecastWorker
    {
        private const int FiveSecondsInMilliseconds = 5000;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object SyncRoot = new object();

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
            Logger.Info(doingWorkLogMessage);
            DateTime timeOfPlannedDelete = DateTime.UtcNow;
            bool previousDeleteDayIsNotTheSameAsThePlannedDelete = this.PreviousDeleteDayIsNotTheSameAsThePlannedDelete(this.previousDelete, timeOfPlannedDelete);
            bool timeOfPlannedDeleteIsAfterScheduledTime = this.TimeOfPlannedDeleteIsAfterScheduledTime(this.scheduledTime, timeOfPlannedDelete);
            if (previousDeleteDayIsNotTheSameAsThePlannedDelete && timeOfPlannedDeleteIsAfterScheduledTime)
            {
                lock (SyncRoot)
                {
                    string checkingDeploymentExistsMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' is checking if deployment for Subscription ID '{1}', Service Name '{2}' and Deployment Slot '{3}' exists.", this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                    Logger.Info(checkingDeploymentExistsMessage);
                    bool deploymentExists = this.deployment.CheckExists(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                    if (deploymentExists)
                    {
                        string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' is deleting deployment for Subscription ID '{1}', Service Name '{2}' and Deployment Slot '{3}' as it was found to exist.", this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                        Logger.Info(deleteDeploymentLogMessage);
                        string deleteRequestId = this.deployment.DeleteRequest(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                        this.WaitForResultOfDeleteRequest(deleteRequestId);
                    }
                    else
                    {
                        string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' is not deleting deployment for Subscription ID '{1}', Service Name '{2}' and Deployment Slot '{3}' as it was not found to exist.", this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                        Logger.Info(deleteDeploymentLogMessage);
                    }

                    // Set the previous delete to this planned delete regardless of whether we actually did the delete.
                    // This is because without doing this, the application will continue to try to delete the application until it is found.
                    // We don't want this to happen, all that has happened is that the deployment did not exist at the scheduled delete time.
                    this.previousDelete = timeOfPlannedDelete;
                }
            }
        }

        private bool PreviousDeleteDayIsNotTheSameAsThePlannedDelete(DateTime timeOfPreviousDelete, DateTime timeOfPlannedDelete)
        {
            if (timeOfPreviousDelete.DayOfYear != timeOfPlannedDelete.DayOfYear)
            {
                string trueLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' has found that the previous delete day is not equal to the planned delete day.", this.Id);
                Logger.Info(trueLogMessage);
                return true;
            }

            string falseLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' has found that the previous delete day is equal to the planned delete day.", this.Id);
            Logger.Info(falseLogMessage);
            return false;
        }

        private bool TimeOfPlannedDeleteIsAfterScheduledTime(DateTime scheduledDeleteTime, DateTime timeOfPlannedDelete)
        {
            if (scheduledDeleteTime.Hour <= timeOfPlannedDelete.Hour && scheduledDeleteTime.Minute <= timeOfPlannedDelete.Minute)
            {
                string trueLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' has found that the schedule delete time is less than the planned delete time.", this.Id);
                Logger.Info(trueLogMessage);
                return true;
            }

            string falseLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' has found that the schedule delete time is greater than or equal to the planned delete time.", this.Id);
            Logger.Info(falseLogMessage);
            return false;
        }

        private void WaitForResultOfDeleteRequest(string deleteRequestId)
        {
            OperationResult operationResult = new OperationResult();
            operationResult.Status = OperationStatus.InProgress;
            bool done = false;
            while (!done)
            {
                operationResult = Operation.StatusCheck(this.subscriptionId, this.certificateThumbprint, deleteRequestId);
                if (operationResult.Status == OperationStatus.InProgress)
                {
                    string logMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' submitted a deployment delete request with ID '{1}', the operation was found to be in process, waiting for '{2}' seconds.", this.Id, deleteRequestId, FiveSecondsInMilliseconds / 1000);
                    Logger.Error(logMessage);
                    Thread.Sleep(FiveSecondsInMilliseconds);
                }
                else
                {
                    done = true;
                }
            }

            if (operationResult.Status == OperationStatus.Failed)
            {
                string logMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' submitted a deployment delete request with ID '{1}' and it failed. The code was '{2}' and message '{3}'.", this.Id, deleteRequestId, operationResult.Code, operationResult.Message);
                Logger.Error(logMessage);
            }
            else if (operationResult.Status == OperationStatus.Succeeded)
            {
                string logMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker with ID '{0}' submitted a deployment delete request with ID '{1}' and it succeeded. The code was '{2}' and message '{3}'.", this.Id, deleteRequestId, operationResult.Code, operationResult.Message);
                Logger.Info(logMessage);
            }
        }
    }
}
