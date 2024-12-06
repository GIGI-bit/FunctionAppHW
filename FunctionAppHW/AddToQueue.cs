using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppHW
{
    public static class AddToQueue
    {
        [Function("AddToQueue")]
        public static async Task Run(
            [TimerTrigger("*/5 * * * * *")] TimerInfo myTimer,
            ILogger log)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string queueName = "productqueue";
            string message = $"Product-{Guid.NewGuid()}";

            QueueClient queueClient = new QueueClient(connectionString, queueName);

            await queueClient.CreateIfNotExistsAsync();

            await queueClient.SendMessageAsync(message);

            log.LogInformation($"Added message to queue: {message} at {DateTime.Now}");
        }
    }
}
