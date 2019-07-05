# Azure.Function.AppInsightAvailabilityTest

[![Build Status](https://dev.azure.com/rabickel/Service%20Mesh/_apis/build/status/rbickel.Azure.Function.AppInsightAvailabilityTest?branchName=master)](https://dev.azure.com/rabickel/Service%20Mesh/_build/latest?definitionId=7&branchName=master)


[![Deployment status](https://vsrm.dev.azure.com/rabickel/_apis/public/Release/badge/797f3aaa-4929-4c47-9706-b15558a958b4/1/1)](https://vsrm.dev.azure.com/rabickel/_apis/public/Release/badge/797f3aaa-4929-4c47-9706-b15558a958b4/1/1)


This project is a proof of concept to run availability tests with an Azure Function and report telemetry to Application Insight. The function uses a timer to run every 5 seconds. This can usually be used to monitor public endpoint, as well as prive endpoints, using an [Azure Functions Premium plan](https://docs.microsoft.com/en-us/azure/azure-functions/functions-premium-plan).


The *Deploy to Azure* button deploys the function in an [Azure Function Consumption plan](https://azure.microsoft.com/en-us/pricing/details/functions/), in order to monitor public endpoints. To start monitoring private endpoint availability, see [Azure Functions Premium plan](https://docs.microsoft.com/en-us/azure/azure-functions/functions-premium-plan).

 [![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

**Function settings**

These settings are required to define the endpoint (uri) to ping, the location and application insight instrumentation key to report the telemetry.

        "AppInsights:instrumentationKey":"b783e117-4fd8-413a-acfd-1234567890"
        "AppInsights:region": "West Europe"
        "Endpoint": "https://www.bing.com/"
        

