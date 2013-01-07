namespace StealFocus.Forecast.Configuration
{
    using StealFocus.Forecast.Configuration.WindowsAzure;
    using StealFocus.Forecast.Configuration.WindowsAzure.HostedService;
    using StealFocus.Forecast.Configuration.WindowsAzure.StorageService;

    public interface IConfigurationSource
    {
        SubscriptionConfiguration GetWindowsAzureSubscriptionConfiguration(string windowsAzureSubscriptionConfigurationId);

        StorageAccountConfiguration GetWindowsAzureStorageAccountConfiguration(string windowsAzureStorageAccountName);

        PackageConfiguration GetWindowsAzurePackageConfiguration(string windowsAzurePackageConfigurationId);

        DeploymentDeleteConfiguration[] GetWindowsAzureDeploymentDeleteConfigurations();

        DeploymentCreateConfiguration[] GetWindowsAzureDeploymentCreateConfigurations();

        ScheduledHorizontalScaleConfiguration[] GetWindowsAzureScheduledHorizontalScaleConfigurations();

        TableDeleteConfiguration[] GetWindowsAzureTableDeleteConfigurations();

        BlobContainerDeleteConfiguration[] GetWindowsAzureBlobContainerDeleteConfigurations();
    }
}
