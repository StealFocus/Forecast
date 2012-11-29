namespace StealFocus.Forecast.Configuration
{
    public interface IConfigurationSource
    {
        WindowsAzureSubscriptionConfiguration GetWindowsAzureSubscriptionConfiguration(string windowsAzureSubscriptionConfigurationId);

        WindowsAzurePackageConfiguration GetWindowsAzurePackageConfiguration(string windowsAzurePackageConfigurationId);

        WindowsAzureDeploymentDeleteConfiguration[] GetWindowsAzureDeploymentDeleteConfigurations();

        WindowsAzureDeploymentCreateConfiguration[] GetWindowsAzureDeploymentCreateConfigurations();

        WindowsAzureTableDeleteConfiguration[] GetWindowsAzureTableDeleteConfigurations();
    }
}
