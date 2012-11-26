﻿namespace StealFocus.Forecast.WindowsAzure
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using log4net;

    using StealFocus.AzureExtensions.HostedService;

    internal class DeploymentDeleteForecastWorker : ForecastWorker
    {
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

        private readonly int pollingIntervalInMinutes;

        private DateTime lastTimeWeDidWork = DateTime.MinValue;

        public DeploymentDeleteForecastWorker(
            string id, 
            IDeployment deployment, 
            IOperation operation, 
            Guid subscriptionId, 
            string certificateThumbprint, 
            string serviceName, 
            string deploymentSlot, 
            TimeSpan dailyStartTime, 
            TimeSpan dailyEndTime, 
            int pollingIntervalInMinutes)
            : base(GetWorkerId(id, serviceName, deploymentSlot))
        {
            this.deployment = deployment;
            this.operation = operation;
            this.subscriptionId = subscriptionId;
            this.certificateThumbprint = certificateThumbprint;
            this.serviceName = serviceName;
            this.deploymentSlot = deploymentSlot;
            this.dailyStartTime = dailyStartTime;
            this.dailyEndTime = dailyEndTime;
            this.pollingIntervalInMinutes = pollingIntervalInMinutes;
        }

        public override void DoWork()
        {
            if (this.lastTimeWeDidWork.AddMinutes(this.pollingIntervalInMinutes) < DateTime.Now)
            {
                string doingWorkLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is doing work.", this.GetType().Name, this.Id);
                Logger.Debug(doingWorkLogMessage);
                bool nowIsInTheSchedule = DetermineIfNowIsInTheSchedule(Logger, this.GetType().Name, this.Id, this.dailyStartTime, this.dailyEndTime);
                if (nowIsInTheSchedule)
                {
                    lock (SyncRoot)
                    {
                        string checkingDeploymentExistsMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is checking if deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' exists.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                        Logger.Debug(checkingDeploymentExistsMessage);
                        bool deploymentExists = this.deployment.CheckExists(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                        if (deploymentExists)
                        {
                            string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is deleting deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' as it was found to exist.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Debug(deleteDeploymentLogMessage);
                            string deleteRequestId = this.deployment.DeleteRequest(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                            ForecastWorker.WaitForResultOfRequest(Logger, this.GetType().Name, this.Id, this.operation, this.subscriptionId, this.certificateThumbprint, deleteRequestId);
                        }
                        else
                        {
                            string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is not deleting deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' as it was not found to exist.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Debug(deleteDeploymentLogMessage);
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

        private static string GetWorkerId(string id, string serviceName, string deploymentSlot)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-{1}-{2}", id, serviceName, deploymentSlot);
        }
    }
}
