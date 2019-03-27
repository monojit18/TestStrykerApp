using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;

namespace TestStrykerApp
{
    public static class ProcessServiceHubTopic
    {
        [FunctionName("ProcessServiceHubTopic")]
        public static void Run([ServiceBusTrigger("strykerdevicetopic", "strykerdevicesubscription",
                                Connection = "STRYKER_SERVICEBUS_CONNECTION")] Message  topicMessage,
                                ILogger log)
        {
            log.LogInformation($"{topicMessage.PartitionKey} - {Encoding.UTF8.GetString(topicMessage.Body)}");
        }
    }
}
