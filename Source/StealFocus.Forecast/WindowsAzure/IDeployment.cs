namespace StealFocus.Forecast.WindowsAzure
{
    using System;

    public interface IDeployment
    {
        bool CheckExists(Guid subscriptionId, string certificateThumbprint, string serviceName, string deploymentSlot);

        /// <param name="subscriptionId">The SubScription ID.</param>
        /// <param name="certificateThumbprint">The certificate thumbprint.</param>
        /// <param name="serviceName">The service name.</param>
        /// <param name="deploymentSlot">Either "Production" or "Staging".</param>
        string DeleteRequest(Guid subscriptionId, string certificateThumbprint, string serviceName, string deploymentSlot);

        /// <param name="subscriptionId">The SubScription ID.</param>
        /// <param name="certificateThumbprint">The certificate thumbprint.</param>
        /// <param name="serviceName">The service name.</param>
        /// <param name="deploymentSlot">Either "Production" or "Staging".</param>
        /// <param name="deploymentName">Shoud not contain spaces.</param>
        /// <param name="packageUrl">The URL to the .cspkg in blob storage.</param>
        /// <param name="label">Limited to 100 characters.</param>
        /// <param name="configurationFilePath">The path to the .cscfg file.</param>
        /// <param name="startDeployment">Whether to start after deployment.</param>
        /// <param name="treatWarningsAsError">Whether to treat warnings as errors.</param>
        string CreateRequest(Guid subscriptionId, string certificateThumbprint, string serviceName, string deploymentSlot, string deploymentName, Uri packageUrl, string label, string configurationFilePath, bool startDeployment, bool treatWarningsAsError);
    }
}
