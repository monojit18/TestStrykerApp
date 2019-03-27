using System;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;

namespace TestStrykerApp
{
    public static class ProcessEventHub
    {
        [FunctionName("ProcessEventHub")]
        public static void Run([EventHubTrigger("%STRYKER_EVENTHUB_NAME%",
                                Connection = "STRYKER_EVENTHUB_CONNECTION",
                                ConsumerGroup = "%STRYKER_EVENTHUB_CONSUMER_GROUP1%")]
                                EventData eventData, ILogger log)
        {
            var bts = eventData.Body.ToArray();
            var str = Encoding.UTF8.GetString(bts);
            log.LogInformation($"{eventData.SystemProperties["x-opt-partition-key"]} - {str}");
            // log.LogInformation($"{str}");

        }
    }
}
