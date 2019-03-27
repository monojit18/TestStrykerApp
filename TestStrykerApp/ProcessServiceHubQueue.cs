using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;

namespace TestStrykerApp
{
    public static class ProcessServiceHubQueue
    {
        [FunctionName("ProcessServiceHubQueue")]
        public static void Run([ServiceBusTrigger("strykerqueue",
                                Connection = "STRYKER_SERVICEBUS_CONNECTION") ]Message queueMessage,
                                ILogger log)
        {
            log.LogInformation($"{queueMessage.PartitionKey} - {Encoding.UTF8.GetString(queueMessage.Body)}");
        }
    }
}
