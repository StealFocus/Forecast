namespace StealFocus.Forecast.WindowsAzure
{
    using System;

    public class DeploymentDeleteForecastWorker : ForecastWorker
    {
        private readonly Guid subscriptionId;

        private readonly string certificateThumbprint;

        private readonly string serviceName;

        private readonly string deploymentSlot;

        private readonly DateTime scheduledTime;

        private bool deleted = false;

        public DeploymentDeleteForecastWorker(Guid subscriptionId, string certificateThumbprint, string serviceName, string deploymentSlot, DateTime scheduledTime)
        {
            this.subscriptionId = subscriptionId;
            this.certificateThumbprint = certificateThumbprint;
            this.serviceName = serviceName;
            this.deploymentSlot = deploymentSlot;
            this.scheduledTime = scheduledTime;
        }

        protected override void DoWork()
        {
            DateTime now = DateTime.UtcNow;
            bool exists = Deployment.CheckExists(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
            if (exists && this.scheduledTime.Hour > now.Hour && this.scheduledTime.Minute > now.Minute && !this.deleted)
            {
                Deployment.DeleteRequest(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
            }
        }
    }
}
