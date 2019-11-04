## App overview

This ASP.NET Core 3.0 app represents a loan application processing pipeline. The following table outlines projects found in the solution.

|Project						  |Description											 |
|---------------------------------|------------------------------------------------------|
|*ContosoLending.CurrencyExchange*|gRPC project handling currency conversion			 |
|*ContosoLending.DomainModel*	  |.NET Standard 2.0 project containing shared models	 |
|*ContosoLending.LoanProcessing*  |Durable Functions project for handling loan processing|
|*ContosoLending.Ui*			  |Server-side Blazor UI project						 |

## Setup

### Install prerequisites

The following software must be installed:

1. [.NET Core SDK version 3.0.100 or later](https://dotnet.microsoft.com/download/dotnet-core/3.0)
1. [Visual Studio 2019 version 16.3 or later](https://visualstudio.microsoft.com/downloads/) with the following workloads:
	1. **ASP.NET and web development**
	1. **Azure development**

### Provision Azure resources

1. Open the [Azure Cloud Shell](https://shell.azure.com) in your web browser.

1. Run the following command to configure your Azure CLI defaults for resource group and region:

	```bash
	az configure --defaults group=<resource_group_name> location=<region_name>
	```

1. Run the following command to provision an Azure Storage account:

	```bash
	az storage account create --name <storage_resource_name>
	```

1. Run the following command to provision an Azure SignalR Service instance:

	```bash
	az signalr create --name <signalr_resource_name> --sku Standard_S1 --service-mode Serverless
	```

### Configure the Azure Functions project

1. Create a new *local.settings.json* file in the root of the *ContosoLending.LoanProcessing* project with the following content:

	```json
	{
	  "IsEncrypted": false,
	  "Values": {
		"AzureSignalRConnectionString": "<signalr_connection_string>",
		"AzureWebJobsStorage": "<storage_connection_string>",
		"FUNCTIONS_WORKER_RUNTIME": "dotnet"
	  },
	  "Host": {
		"CORS": "https://localhost:44364",
		"CORSCredentials": true,
		"LocalHttpPort": 7071
	  }
	}
	```

1. From the Azure Cloud Shell, run the following command to get the Azure Storage account's connection string:

	```bash
	az storage account show-connection-string --name <storage_resource_name> --query connectionString
	```

	Copy the resulting value (without the double quotes) to your clipboard.

1. Replace "&lt;storage_connection_string&gt;" in *local.settings.json* with the value on your clipboard.

1. Run the following command to get the Azure SignalR Service's connection string:

	```bash
	az signalr key list --name <signalr_resource_name> --query primaryConnectionString
	```

	Copy the resulting value (without the double quotes) to your clipboard.

1. Replace "&lt;signalr_connection_string&gt;" in *local.settings.json* with the value on your clipboard.

## Testing

1. Open the solution file (*src\ContosoLending.sln*).
1. In **Solution Explorer**, right-click the *libman.json* file in the **ContosoLending.Ui** project > **Restore Client-Side Libraries**.
1. In **Solution Explorer**, right-click the solution name > **Properties**.
1. Select the **Multiple startup projects** radio button, and configure the solution as follows:

	![multiple project launch configuration in Visual Studio](https://user-images.githubusercontent.com/10702007/68152936-39716780-ff0a-11e9-9f62-babf2267ef77.png)

1. Select the **OK** button.
1. Select the **Start** button next to the **&lt;Multiple Startup Projects&gt;** launch configuration drop-down list.
