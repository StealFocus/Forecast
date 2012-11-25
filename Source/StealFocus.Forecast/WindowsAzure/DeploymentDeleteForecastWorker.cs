namespace StealFocus.Forecast.WindowsAzure
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;

    using log4net;

    using StealFocus.AzureExtensions.HostedService;

    internal class DeploymentDeleteForecastWorker : ForecastWorker
    {
        private const int FiveSecondsInMilliseconds = 5000;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object SyncRoot = new object();

        private readonly IDeployment deployment;

        private readonly IOperation operation;

        private readonly Guid subscriptionId;

        private readonly string certificateThumbprint;

        private readonly string serviceName;

        private readonly string deploymentSlot;

        private readonly TimeSpan dailyStartTime;

        private readonly TimeSpan dailyEndTime;

        public DeploymentDeleteForecastWorker(string id, IDeployment deployment, IOperation operation, Guid subscriptionId, string certificateThumbprint, string serviceName, string deploymentSlot, TimeSpan dailyStartTime, TimeSpan dailyEndTime, int pollingIntervalInMinutes)
            : base(GetWorkerId(id, serviceName, deploymentSlot), pollingIntervalInMinutes * 60 * 1000)
        {
            this.deployment = deployment;
            this.operation = operation;
            this.subscriptionId = subscriptionId;
            this.certificateThumbprint = certificateThumbprint;
            this.serviceName = serviceName;
            this.deploymentSlot = deploymentSlot;
            this.dailyStartTime = dailyStartTime;
            this.dailyEndTime = dailyEndTime;
        }

        public override void DoWork()
        {
            string doingWorkLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker '{0}' is doing work.", this.Id);
            Logger.Debug(doingWorkLogMessage);
            bool nowIsInTheSchedule = this.DetermineIfNowIsInTheSchedule();
            if (nowIsInTheSchedule)
            {
                lock (SyncRoot)
                {
                    string checkingDeploymentExistsMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker '{0}' is checking if deployment for Subscription ID '{1}', Service Name '{2}' and Deployment Slot '{3}' exists.", this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                    Logger.Debug(checkingDeploymentExistsMessage);
                    bool deploymentExists = this.deployment.CheckExists(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                    if (deploymentExists)
                    {
                        string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker '{0}' is deleting deployment for Subscription ID '{1}', Service Name '{2}' and Deployment Slot '{3}' as it was found to exist.", this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                        Logger.Debug(deleteDeploymentLogMessage);
                        string deleteRequestId = this.deployment.DeleteRequest(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                        this.WaitForResultOfDeleteRequest(deleteRequestId);
                    }
                    else
                    {
                        string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker '{0}' is not deleting deployment for Subscription ID '{1}', Service Name '{2}' and Deployment Slot '{3}' as it was not found to exist.", this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                        Logger.Debug(deleteDeploymentLogMessage);
                    }
                }
            }
        }

        private static string GetWorkerId(string id, string serviceName, string deploymentSlot)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-{1}-{2}", id, serviceName, deploymentSlot);
        }

        private bool DetermineIfNowIsInTheSchedule()
        {
            DateTime startTimeOfScheduleToday = DateTime.Today.Add(this.dailyStartTime);
            DateTime endTimeOfScheduleToday = DateTime.Today.Add(this.dailyEndTime);
            DateTime now = DateTime.Now;
            bool result;
            if (startTimeOfScheduleToday < now && now < endTimeOfScheduleToday)
            {
                string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker '{0}' has found that now ('{1}') falls into the schedule with start time of '{2}' and end time of '{3}'.", this.Id, now, this.dailyStartTime, this.dailyEndTime);
                Logger.Debug(deleteDeploymentLogMessage);
                result = true;
            }
            else
            {
                string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker '{0}' has found that now ('{1}') does not fall into the schedule with start time of '{2}' and end time of '{3}'.", this.Id, now, this.dailyStartTime, this.dailyEndTime);
                Logger.Debug(deleteDeploymentLogMessage);
                result = false;
            }

            return result;
        }

        private void WaitForResultOfDeleteRequest(string deleteRequestId)
        {
            OperationResult operationResult = new OperationResult();
            operationResult.Status = OperationStatus.InProgress;
            bool done = false;
            while (!done)
            {
                operationResult = this.operation.StatusCheck(this.subscriptionId, this.certificateThumbprint, deleteRequestId);
                if (operationResult.Status == OperationStatus.InProgress)
                {
                    string logMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker '{0}' submitted a deployment delete request with ID '{1}', the operation was found to be in process, waiting for '{2}' seconds.", this.Id, deleteRequestId, FiveSecondsInMilliseconds / 1000);
                    Logger.Debug(logMessage);
                    Thread.Sleep(FiveSecondsInMilliseconds);
                }
                else
                {
                    done = true;
                }
            }

            if (operationResult.Status == OperationStatus.Failed)
            {
                string logMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker '{0}' submitted a deployment delete request with ID '{1}' and it failed. The code was '{2}' and message '{3}'.", this.Id, deleteRequestId, operationResult.Code, operationResult.Message);
                Logger.Error(logMessage);
            }
            else if (operationResult.Status == OperationStatus.Succeeded)
            {
                string logMessage = string.Format(CultureInfo.CurrentCulture, "DeploymentDeleteForecastWorker '{0}' submitted a deployment delete request with ID '{1}' and it succeeded. The code was '{2}' and message '{3}'.", this.Id, deleteRequestId, operationResult.Code, operationResult.Message);
                Logger.Info(logMessage);
            }
        }
    }
}
