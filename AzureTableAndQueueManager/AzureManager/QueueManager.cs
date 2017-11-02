using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureManager.Properties;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Queue; // Namespace for Queue storage types
using System.Configuration;
using Newtonsoft.Json;

namespace AzureManager
{
    public static class QueueManager
    {
        private static CloudStorageAccount _storageAccount;
        private static CloudQueueClient _queueClient;
        private static CloudQueue _queue;

        public static void Initialize()
        {
            _storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            _queueClient = _storageAccount.CreateCloudQueueClient();
            CreateQueueIfNotExist();
        }

        private static void CreateQueueIfNotExist()
        {
            // Retrieve a reference to a container.
            _queue = _queueClient.GetQueueReference(ConfigurationManager.AppSettings["QueueName"]);

            // Create the queue if it doesn't already exist
            _queue.CreateIfNotExists();
        }

        public static Task AddMessage(object messageObject)
        {

            _queue = _queueClient.GetQueueReference(ConfigurationManager.AppSettings["QueueName"]);
            string serializedMessage = JsonConvert.SerializeObject(messageObject);
            CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(serializedMessage);
            return _queue.AddMessageAsync(cloudQueueMessage);
        }

        public static string PeekMessage()
        {
            return _queue.PeekMessage().AsString;
        }

        public static CloudQueueMessage GetMessage()
        {
            return _queue.GetMessage();
        }

        public static List<CloudQueueMessage> GetMessages()
        {
            return _queue.GetMessages(10, System.TimeSpan.FromSeconds(1)).ToList();
        }

        public static string DeleteMessage(CloudQueueMessage message)
        {
            var deletedMessage = message.AsString;
            _queue.DeleteMessage(message);
            return deletedMessage;
        }
    }
}
