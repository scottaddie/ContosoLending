## Setup

### Configure Visual Studio workloads

A minimum of Visual Studio 2019 version 16.3 is required, along with the following workloads:

1. **ASP.NET and web development**
1. **Azure development**

### Provision required Azure resources

1. Navigate to the Azure Portal in your web browser, and launch the Azure Cloud Shell.

1. Run the following command to configure your Azure CLI defaults for resource group and region:

	```bash
	az configure --defaults group=ConferenceTalks location=centralus
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

To run the app locally, configure the solution to launch the following 3 projects:

1. *ContosoLending.CurrencyExchange*
1. *ContosoLending.LoanProcessing*
1. *ContosoLending.Ui*

For example:

![multiple project launch configuration in Visual Studio](https://user-images.githubusercontent.com/10702007/68152936-39716780-ff0a-11e9-9f62-babf2267ef77.png)
