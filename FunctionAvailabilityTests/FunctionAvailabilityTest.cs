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
using Newtonsoft.Json;

namespace FunctionAvailabilityTests
{
    public static class FunctionAvailabilityTest
    {
        private static readonly string InstrumentationKey = Environment.GetEnvironmentVariable("AppInsights:instrumentationKey");
        private static readonly string Region = Environment.GetEnvironmentVariable("AppInsights:region");
        private static readonly string Endpoint = Environment.GetEnvironmentVariable("endpoint");
        private static readonly Dictionary<string, string> Properties = new Dictionary<string, string>();
        private static readonly TelemetryConfiguration conf = new TelemetryConfiguration(InstrumentationKey);
        private static readonly TelemetryClient telemetryClient = new TelemetryClient(conf);

        private static HttpClient httpClient = new HttpClient();

        static FunctionAvailabilityTest()
        {
            try
            {
                Properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(Environment.GetEnvironmentVariable("Properties"));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception while deserializing Properties :{ex.Message}");
            }
        }

        [FunctionName("FunctionAvailabilityTest")]
        public static async Task Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log, Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            log.LogInformation($"Test DevOps");

            Stopwatch sw = Stopwatch.StartNew();
            var response = await httpClient.GetAsync(Endpoint);
            sw.Stop();

            AvailabilityTelemetry avtel = new AvailabilityTelemetry()
            {
                Name = Endpoint,
                Duration = sw.Elapsed,
                Success = response.IsSuccessStatusCode,
                RunLocation = Region,
                Timestamp = DateTimeOffset.Now
            };
            foreach (var p in Properties)
                avtel.Properties.Add(p);

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
