using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppHW
{
    public static class ReadAndDeleteFromQueue
    {
        [Function("ReadAndDeleteFromQueue")]
        public static async Task Run(
            [TimerTrigger("*/7 * * * * *")] TimerInfo myTimer,
            ILogger log)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string queueName = "productqueue";

            var queueClient = new Azure.Storage.Queues.QueueClient(connectionString, queueName);

            if (await queueClient.ExistsAsync())
            {
                QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: 1);

                foreach (var message in messages)
                {
                    log.LogInformation($"Read message: {message.MessageText}");

                    await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                    log.LogInformation($"Deleted message with ID: {message.MessageId}");
                }
            }
            else
            {
                log.LogWarning($"Queue '{queueName}' does not exist.");
            }
        }
    }
}
