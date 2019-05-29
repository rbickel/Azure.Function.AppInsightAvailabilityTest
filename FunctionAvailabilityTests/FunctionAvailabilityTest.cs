using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionAvailabilityTests
{
    public static class FunctionAvailabilityTest
    {
        private static readonly string InstrumentationKey = Environment.GetEnvironmentVariable("AppInsights:instrumentationKey");
        private static readonly string Region = Environment.GetEnvironmentVariable("AppInsights:region");
        private static readonly string Endpoint = Environment.GetEnvironmentVariable("endpoint");
        private static readonly TelemetryConfiguration conf = new TelemetryConfiguration(InstrumentationKey);
        private static readonly TelemetryClient telemetryClient = new TelemetryClient(conf);

        private static HttpClient httpClient = new HttpClient();

        [FunctionName("FunctionAvailabilityTest")]
        public static async Task Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log, Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            log.LogInformation($"Test DevOps");

            Stopwatch sw = Stopwatch.StartNew();
            var response = await httpClient.GetAsync(Endpoint);
            sw.Stop();

            AvailabilityTelemetry avtel = new AvailabilityTelemetry
            {
                Name = Endpoint,
                Duration = sw.Elapsed,
                Success = response.IsSuccessStatusCode,
                RunLocation = Region,
                Timestamp = DateTimeOffset.Now
            };

            log.LogInformation($"Response {response.StatusCode} for {Endpoint} in {sw.ElapsedMilliseconds}ms");

            if (!avtel.Success)
            {
                avtel.Message = response.ReasonPhrase;
            }

            telemetryClient.TrackAvailability(avtel);
            log.LogInformation($"Pushed to telemetry");

            telemetryClient.Flush();
        }
    }
}
