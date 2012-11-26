<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="74734de7-e148-488f-944a-a85707079ec6" namespace="StealFocus.Forecast.Configuration" xmlSchemaNamespace="urn:StealFocus.Forecast.Configuration" assemblyName="StealFocus.Forecast.Configuration" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="StealFocusForecastConfiguration" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="stealFocusForecastConfiguration">
      <elementProperties>
        <elementProperty name="WindowsAzureSubscriptions" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="windowsAzureSubscriptions" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureSubscriptionConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="WindowsAzurePackages" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="windowsAzurePackages" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzurePackageConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="WindowsAzureDeploymentDeletes" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="windowsAzureDeploymentDeletes" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureDeploymentDeleteConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="WindowsAzureDeploymentCreates" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="windowsAzureDeploymentCreates" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureDeploymentCreateConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="WindowsAzureSubscriptionConfigurationElementCollection" xmlItemName="windowsAzureSubscription" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureSubscriptionConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WindowsAzureSubscriptionConfigurationElement">
      <attributeProperties>
        <attributeProperty name="SubscriptionId" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="subscriptionId" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="CertificateThumbprint" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="certificateThumbprint" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="WindowsAzurePackageConfigurationElementCollection" xmlItemName="windowsAzurePackage" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzurePackageConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WindowsAzurePackageConfigurationElement">
      <attributeProperties>
        <attributeProperty name="StorageAccountName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="storageAccountName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ContainerName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="containerName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="BlobName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="blobName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="WindowsAzureDeploymentDeleteConfigurationElementCollection" xmlItemName="windowsAzureDeploymentDelete" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureDeploymentDeleteConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WindowsAzureDeploymentDeleteConfigurationElement">
      <attributeProperties>
        <attributeProperty name="ServiceName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="serviceName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="SubscriptionConfigurationId" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="subscriptionConfigurationId" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="PollingIntervalInMinutes" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="pollingIntervalInMinutes" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="DeploymentSlots" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="deploymentSlots" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/DeploymentSlotConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Schedules" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="schedules" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/ScheduleConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="DeploymentSlotConfigurationElementCollection" xmlItemName="deploymentSlot" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/DeploymentSlotConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="DeploymentSlotConfigurationElement">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="ScheduleConfigurationElementCollection" xmlItemName="schedule" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/ScheduleConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ScheduleConfigurationElement">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Days" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="days" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/DayConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="WindowsAzureDeploymentCreateConfigurationElementCollection" xmlItemName="windowsAzureDeploymentCreate" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureDeploymentCreateConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WindowsAzureDeploymentCreateConfigurationElement">
      <attributeProperties>
        <attributeProperty name="ServiceName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="serviceName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="SubscriptionConfigurationId" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="subscriptionConfigurationId" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="PollingIntervalInMinutes" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="pollingIntervalInMinutes" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="DeploymentSlot" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="deploymentSlot" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="WindowsAzurePackageId" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="windowsAzurePackageId" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="DeploymentName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="deploymentName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="DeploymentLabel" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="deploymentLabel" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="PackageConfigurationFilePath" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="packageConfigurationFilePath" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartDeployment" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="startDeployment" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/Boolean" />
          </type>
        </attributeProperty>
        <attributeProperty name="TreatWarningsAsError" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="treatWarningsAsError" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Schedules" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="schedules" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/ScheduleConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="DayConfigurationElementCollection" xmlItemName="day" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/DayConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="DayConfigurationElement">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="StartTime" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="startTime" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/TimeSpan" />
          </type>
        </attributeProperty>
        <attributeProperty name="EndTime" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="endTime" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/TimeSpan" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>