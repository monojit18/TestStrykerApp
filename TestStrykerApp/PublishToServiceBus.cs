using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.ServiceBus;


namespace TestStrykerApp
{
    public static class PublishToServiceBus
    {

        private static async Task PushMessagesToQueueAsync()
        {

            var serviceBusConnectionString = Environment.GetEnvironmentVariable(
                                                         "STRYKER_SERVICEBUS_CONNECTION");

            var queueNameString = Environment.GetEnvironmentVariable("STRYKER_SERVICEBUS_QUEUE_NAME");
            var connectionBuilder = new ServiceBusConnectionStringBuilder(
                                        serviceBusConnectionString)
            {

                EntityPath = queueNameString

            };

            var messageQueueClient = new QueueClient(connectionBuilder);

            for (int i = 0; i < 30; ++i)
            {

                string partitionKeyString = string.Empty;

                if (i < 10)
                    partitionKeyString = "P1";
                else if (i >= 10 && i < 20)
                    partitionKeyString = "P2";
                else if (i >= 20 && i < 30)
                    partitionKeyString = "P3";

                var message = new Message()
                {

                    Body = Encoding.UTF8.GetBytes($"Queue {i.ToString()}"),
                    PartitionKey = partitionKeyString

                };

                await messageQueueClient.SendAsync(message);

            }

            await messageQueueClient.CloseAsync();

        }

        private static async Task PushMessagesToTopicAsync()
        {

            var serviceBusConnectionString = Environment.GetEnvironmentVariable(
                                                         "STRYKER_SERVICEBUS_CONNECTION");

            var queueNameString = Environment.GetEnvironmentVariable("STRYKER_SERVICEBUS_TOPIC_NAME");
            var connectionBuilder = new ServiceBusConnectionStringBuilder(
                                        serviceBusConnectionString)
            {

                EntityPath = queueNameString

            };

            var messageTopicClient = new TopicClient(connectionBuilder);

            for (int i = 0; i < 30; ++i)
            {

                string partitionKeyString = string.Empty;

                if (i < 10)
                    partitionKeyString = "P1";
                else if (i >= 10 && i < 20)
                    partitionKeyString = "P2";
                else if (i >= 20 && i < 30)
                    partitionKeyString = "P3";

                var message = new Message()
                {

                    Body = Encoding.UTF8.GetBytes($"Topic {i.ToString()}"),
                    PartitionKey = partitionKeyString

                };

                await messageTopicClient.SendAsync(message);

            }

            await messageTopicClient.CloseAsync();

        }


        [FunctionName("PublishToServiceBus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            await PushMessagesToQueueAsync();
            await PushMessagesToTopicAsync();

            return (ActionResult)new OkObjectResult(true);

        }
    }
}
