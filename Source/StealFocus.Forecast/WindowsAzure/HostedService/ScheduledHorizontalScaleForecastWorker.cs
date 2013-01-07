namespace StealFocus.Forecast.WindowsAzure.HostedService
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    using log4net;

    using StealFocus.AzureExtensions.HostedService;
    using StealFocus.Forecast.Configuration.WindowsAzure.HostedService;

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

        private readonly ScheduleDay[] scheduleDays;

        private readonly string roleName;

        private readonly int instanceCount;

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
            ScheduleDay[] scheduleDays,
            string roleName,
            int instanceCount,
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
            this.scheduleDays = scheduleDays;
            this.roleName = roleName;
            this.instanceCount = instanceCount;
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
                            string createDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is horizontally scaling deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Info(createDeploymentLogMessage);
                            try
                            {
                                XDocument configuration = this.deployment.GetConfiguration(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot);
                                XDocument updatedConfiguration = UpdateConfiguration(configuration, this.roleName, this.instanceCount);
                                string changeConfigurationRequestId = this.deployment.ChangeConfiguration(this.subscriptionId, this.certificateThumbprint, this.serviceName, this.deploymentSlot, updatedConfiguration, this.treatWarningsAsError, this.mode);
                                this.WaitForResultOfRequest(Logger, this.GetType().Name, this.operation, this.subscriptionId, this.certificateThumbprint, changeConfigurationRequestId);
                            }
                            catch (Exception e)
                            {
                                string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' experienced an error horizontally scaling deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}'. The operation will be retried after the next polling interval.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                                Logger.Error(errorMessage, e);
                            }
                        }
                        else
                        {
                            string createDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is not horizontally scaling deployment for Subscription ID '{2}', Service Name '{3}' and Deployment Slot '{4}' as it was not found to exist.", this.GetType().Name, this.Id, this.subscriptionId, this.serviceName, this.deploymentSlot);
                            Logger.Warn(createDeploymentLogMessage);
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

        private static XDocument UpdateConfiguration(XDocument configuration, string roleName, int instanceCount)
        {
            XNamespace ns = XmlNamespace.ServiceHosting200810ServiceConfiguration;
            XElement configurationRootElement = configuration.Root;
            if (configurationRootElement == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The configuration XML returned from the management API did not contain a root element.");
                throw new ForecastException(exceptionMessage);
            }

            XElement instancesElement = (from role in configurationRootElement.Elements(ns + "Role")
                                         where role.Attribute("name").Value == roleName
                                         select role).Single().Element(ns + "Instances");
            if (instancesElement == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The configuration XML returned from the management API did not contain a 'Role' element matching name '{0}'.", roleName);
                throw new ForecastException(exceptionMessage);
            }

            instancesElement.Attribute("count").SetValue(instanceCount);
            return configuration;
        }
    }
}
