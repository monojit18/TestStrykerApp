using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.EventHubs;

namespace PushToEventHub
{
    public static class PushToEventHub
    {

        private static async Task PushEventAsync()
        {

            var eventhubConnectionString = Environment.GetEnvironmentVariable(
                                                "STRYKER_EVENTHUB_CONNECTION");

            var eventhubNameString = Environment.GetEnvironmentVariable("STRYKER_EVENTHUB_NAME");
            var eventhubBuilder = new EventHubsConnectionStringBuilder(eventhubConnectionString)
            {

                EntityPath = eventhubNameString

            };

            var eventHubClient = EventHubClient.CreateFromConnectionString(
                                 eventhubBuilder.ToString());                     

            for (int i = 0;i < 30;++i)
            {

                var eventData = new EventData(Encoding.UTF8.GetBytes($"Message {i}"));
                if (i < 10)
                    await eventHubClient.SendAsync(eventData, "P1");
                else if (i >= 10 && i < 20)
                    await eventHubClient.SendAsync(eventData, "P2");
                else if (i >= 20 && i < 30)
                    await eventHubClient.SendAsync(eventData, "P3");

            }

            await eventHubClient.CloseAsync();

        }


        [FunctionName("PushToEventHub")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");
            await PushEventAsync();
            return (ActionResult)new OkObjectResult(true);

        }
    }
}
