using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureManager;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureManager.UsageApp
{
    class Program
    {
        static void Main(string[] args)
        {
            QueueManager.Initialize();
            QueueManager.AddMessage("test1");
            QueueManager.AddMessage("Test2");
            Console.WriteLine(QueueManager.PeekMessage());
            ShowMessages();
            DeleteMessages();
            Console.WriteLine("End");
            Console.ReadLine();
        }

        private static void DeleteMessages()
        {
            var messageToDelete = QueueManager.GetMessage();
            var count = 0;
            while (count <= 5)
            {
                messageToDelete = QueueManager.GetMessage();
                Console.WriteLine("No Message...Sleeping count: " + count);
                Thread.Sleep(1000);
                count++; //FailSafe
                if (messageToDelete != null)
                {
                    Console.WriteLine("Deleted message: " + QueueManager.DeleteMessage(messageToDelete).ToString());
                    count = 0;
                }
            }
        }

        private static void ShowMessages()
        {
            var Messages = QueueManager.GetMessages();
            Console.WriteLine("Messages");
            foreach (var message in Messages)
            {
                Console.WriteLine(message.AsString);
            }
        }
    }
}
