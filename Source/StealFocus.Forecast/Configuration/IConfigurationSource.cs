namespace StealFocus.Forecast.Configuration
{
    public interface IConfigurationSource
    {
        WindowsAzureSubscriptionConfiguration GetWindowsAzureSubscriptionConfiguration(string windowsAzureSubscriptionConfigurationId);

        WindowsAzureStorageAccountConfiguration GetWindowsAzureStorageAccountConfiguration(string windowsAzureStorageAccountName);

        WindowsAzurePackageConfiguration GetWindowsAzurePackageConfiguration(string windowsAzurePackageConfigurationId);

        WindowsAzureDeploymentDeleteConfiguration[] GetWindowsAzureDeploymentDeleteConfigurations();

        WindowsAzureDeploymentCreateConfiguration[] GetWindowsAzureDeploymentCreateConfigurations();

        WindowsAzureTableDeleteConfiguration[] GetWindowsAzureTableDeleteConfigurations();
    }
}
