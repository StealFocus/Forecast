// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   GlobalSuppressions.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "StealFocus.Forecast.Configuration.WindowsAzureSubscriptionConfigurationElementCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "StealFocus.Forecast.Configuration.WindowsAzurePackageConfigurationElementCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "StealFocus.Forecast.Configuration.WindowsAzureDeploymentDeleteConfigurationElementCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "StealFocus.Forecast.Configuration.WindowsAzureDeploymentCreateConfigurationElementCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "StealFocus.Forecast.Configuration.ScheduleConfigurationElementCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "StealFocus.Forecast.Configuration.DeploymentSlotConfigurationElementCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "StealFocus.Forecast.Configuration.DayConfigurationElementCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "StealFocus.Forecast.Configuration.HorizontalScaleConfigurationElementCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "StealFocus.Forecast.Configuration.ServiceConfigurationElementCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "StealFocus.Forecast")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "StealFocus.Forecast.Configuration.DayConfigurationElement.#GetDayOfWeek()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "StealFocus.Forecast.Configuration.WindowsAzureSubscriptionConfigurationElement.#GetWindowsAzureSubscriptionId()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "StealFocus.Forecast.Configuration.IConfigurationSource.#GetWindowsAzureHostedServiceWhiteListConfiguration()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "StealFocus.Forecast.WindowsAzure.StorageService.TableDeleteForecastWorker.#DoWork()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "StealFocus.Forecast.WindowsAzure.HostedService.DeploymentDeleteForecastWorker.#DoWork()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "StealFocus.Forecast.WindowsAzure.HostedService.DeploymentCreateForecastWorker.#DoWork()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "StealFocus.Forecast.WindowsAzure.StorageService.BlobContainerDeleteForecastWorker.#DoWork()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "StealFocus.Forecast.WindowsAzure.HostedService.ScheduledHorizontalScaleForecastWorker.#DoWork()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "StealFocus.Forecast.WindowsAzure.HostedService.WhiteListForecastWorker.#DeleteDeployedService(StealFocus.AzureExtensions.ISubscription,System.String,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)", Scope = "member", Target = "StealFocus.Forecast.Program.#RunAsConsole()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "configurationSourceType", Scope = "member", Target = "StealFocus.Forecast.Configuration.StealFocusForecastConfiguration.#GetConfigurationSource()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DayOfWeek", Scope = "member", Target = "StealFocus.Forecast.Configuration.DayConfigurationElement.#GetDayOfWeek()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "StealFocus", Scope = "member", Target = "StealFocus.Forecast.Configuration.StealFocusForecastConfiguration.#GetConfigurationSource()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "StealFocus.Forecast.Configuration.RoleConfigurationElementCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "StealFocus.Forecast.WindowsAzure.HostedService.WhiteListForecastWorker.#HorizontallyScaleDeployedServiceRole(StealFocus.AzureExtensions.ISubscription,System.String,System.String,StealFocus.AzureExtensions.HostedService.HorizontalScale[])")]
