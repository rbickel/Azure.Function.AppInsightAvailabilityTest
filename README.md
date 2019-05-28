# Azure.Function.AppInsightAvailabilityTest

This prohect is a proof of concept to run availability tests with an Azure Function and report telemetry to Application Insight. The function uses a timer to run every second. This can usually be used to monitor public endpoint, as well as prive endpoints, using an [Azure Functions Premium plan](https://docs.microsoft.com/en-us/azure/azure-functions/functions-premium-plan).


The *Deploy to Azure* button deploys the function in an Azure Function Consumption plan, in order to monitor public endpoints. To start monitoring private endpoint availability, see [Azure Functions Premium plan](https://docs.microsoft.com/en-us/azure/azure-functions/functions-premium-plan).

 [![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

**Function settings**

These settings are required to define the endpoint (uri) to ping, the location and application insight instrumentation key to report the telemetry.

        "AppInsights:instrumentationKey":"b783e117-4fd8-413a-acfd-1234567890"
        "AppInsights:region": "West Europe"
        "Endpoint": "https://myendpointtotest.com/"
        

