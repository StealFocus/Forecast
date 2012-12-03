namespace StealFocus.Forecast.Configuration
{
    using StealFocus.Forecast.Configuration.WindowsAzure;

    public interface IConfigurationSource
    {
        SubscriptionConfiguration GetWindowsAzureSubscriptionConfiguration(string windowsAzureSubscriptionConfigurationId);

        StorageAccountConfiguration GetWindowsAzureStorageAccountConfiguration(string windowsAzureStorageAccountName);

        PackageConfiguration GetWindowsAzurePackageConfiguration(string windowsAzurePackageConfigurationId);

        DeploymentDeleteConfiguration[] GetWindowsAzureDeploymentDeleteConfigurations();

        DeploymentCreateConfiguration[] GetWindowsAzureDeploymentCreateConfigurations();

        TableDeleteConfiguration[] GetWindowsAzureTableDeleteConfigurations();

        BlobContainerDeleteConfiguration[] GetWindowsAzureBlobContainerDeleteConfigurations();
    }
}
