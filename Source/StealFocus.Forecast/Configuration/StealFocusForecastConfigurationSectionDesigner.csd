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
    <configurationSection name="StealFocusForecastConfiguration" accessModifier="Internal" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="stealFocusForecastConfiguration">
      <attributeProperties>
        <attributeProperty name="CustomConfigurationSourceType" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="customConfigurationSourceType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="ScheduleDefinitions" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="scheduleDefinitions" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/ScheduleDefinitionConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="WindowsAzure" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="windowsAzure" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureConfigurationElement" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="WindowsAzureSubscriptionConfigurationElementCollection" accessModifier="Internal" xmlItemName="subscription" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureSubscriptionConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WindowsAzureSubscriptionConfigurationElement" accessModifier="Internal">
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
    <configurationElementCollection name="WindowsAzurePackageConfigurationElementCollection" accessModifier="Internal" xmlItemName="package" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzurePackageConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WindowsAzurePackageConfigurationElement" accessModifier="Internal">
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
    <configurationElementCollection name="WindowsAzureDeploymentDeleteConfigurationElementCollection" accessModifier="Internal" xmlItemName="deploymentDelete" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureDeploymentDeleteConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WindowsAzureDeploymentDeleteConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="ServiceName" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="serviceName" isReadOnly="false">
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
        <elementProperty name="Schedules" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="schedules" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/ScheduleConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="DeploymentSlotConfigurationElementCollection" accessModifier="Internal" xmlItemName="deploymentSlot" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/DeploymentSlotConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="DeploymentSlotConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="ScheduleConfigurationElementCollection" accessModifier="Internal" xmlItemName="schedule" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/ScheduleConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ScheduleConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="ScheduleDefinitionName" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="scheduleDefinitionName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="WindowsAzureDeploymentCreateConfigurationElementCollection" accessModifier="Internal" xmlItemName="deploymentCreate" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureDeploymentCreateConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WindowsAzureDeploymentCreateConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="ServiceName" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="serviceName" isReadOnly="false">
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
        <elementProperty name="Schedules" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="schedules" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/ScheduleConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="DayConfigurationElementCollection" accessModifier="Internal" xmlItemName="day" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/DayConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="DayConfigurationElement" accessModifier="Internal">
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
    <configurationElementCollection name="ScheduleDefinitionConfigurationElementCollection" accessModifier="Internal" xmlItemName="scheduleDefinition" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/ScheduleDefinitionConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ScheduleDefinitionConfigurationElement" accessModifier="Internal">
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
    <configurationElementCollection name="WindowsAzureTableDeleteConfigurationElementCollection" accessModifier="Internal" xmlItemName="tableDelete" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureTableDeleteConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WindowsAzureTableDeleteConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="StorageAccountName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="storageAccountName" isReadOnly="false">
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
        <elementProperty name="StorageTables" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="storageTables" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/StorageTableConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Schedules" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="schedules" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/ScheduleConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="StorageTableConfigurationElementCollection" accessModifier="Internal" xmlItemName="storageTable" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/StorageTableConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="StorageTableConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="tableName" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="tableName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="WindowsAzureStorageAccountConfigurationElementCollection" accessModifier="Internal" xmlItemName="storageAccount" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureStorageAccountConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WindowsAzureStorageAccountConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="storageAccountName" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="storageAccountName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="storageAccountKey" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="storageAccountKey" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="WindowsAzureBlobContainerDeleteConfigurationElementCollection" accessModifier="Internal" xmlItemName="blobContainerDelete" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureBlobContainerDeleteConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="WindowsAzureBlobContainerDeleteConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="StorageAccountName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="storageAccountName" isReadOnly="false">
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
        <elementProperty name="BlobContainers" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="blobContainers" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/BlobContainerConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Schedules" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="schedules" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/ScheduleConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="BlobContainerConfigurationElementCollection" accessModifier="Internal" xmlItemName="blobContainer" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/BlobContainerConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="BlobContainerConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="blobContainerName" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="blobContainerName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/74734de7-e148-488f-944a-a85707079ec6/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="WindowsAzureConfigurationElement" accessModifier="Internal">
      <elementProperties>
        <elementProperty name="Subscriptions" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="subscriptions" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureSubscriptionConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="HostedService" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="hostedService" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureHostedServiceConfigurationElement" />
          </type>
        </elementProperty>
        <elementProperty name="StorageService" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="storageService" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureStorageServiceConfigurationElement" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="WindowsAzureHostedServiceConfigurationElement" accessModifier="Internal">
      <elementProperties>
        <elementProperty name="Packages" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="packages" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzurePackageConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="DeploymentDeletes" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="deploymentDeletes" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureDeploymentDeleteConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="DeploymentCreates" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="deploymentCreates" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureDeploymentCreateConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="WindowsAzureStorageServiceConfigurationElement" accessModifier="Internal">
      <elementProperties>
        <elementProperty name="StorageAccounts" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="storageAccounts" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureStorageAccountConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="TableDeletes" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="tableDeletes" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureTableDeleteConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="BlobContainerDeletes" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="blobContainerDeletes" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/74734de7-e148-488f-944a-a85707079ec6/WindowsAzureBlobContainerDeleteConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>