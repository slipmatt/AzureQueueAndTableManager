using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureManager;
using AzureManager.TableEntities;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureManager.UsageApp
{
    class Program
    {
        static void Main(string[] args)
        {
            QueueUsage();
            Console.ReadLine();

            TableUsage();
            Console.ReadLine();
        }

        private static void TableUsage()
        {
            MyEntity entity = new MyEntity
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = "5",
                TrackId = 5,
                DateInserted = DateTime.Now.ToShortDateString(),
                ETag = "*"
            };
            Console.WriteLine("Table CreateIfExists Executing");
            var tm = new TableManager<MyEntity>("TestTable");
            Console.WriteLine("Table CreateIfExists Done");
            Console.WriteLine("Record Write Executing");
            tm.Write(entity);
            Console.WriteLine("Record Write Done");
            Console.WriteLine("Record Update Executing");
            entity.TrackId = 6;
            tm.Update(entity);
            Console.WriteLine("Record Update Done");
            Console.WriteLine("Record Delete Executing");
            tm.Delete(entity);
            Console.WriteLine("Record Delete Done");
            Console.WriteLine("End Tables, hit enter");
        }

        private static void QueueUsage()
        {
            QueueManager.Initialize();
            QueueManager.AddMessage("test1");
            QueueManager.AddMessage("Test2");
            Console.WriteLine(QueueManager.PeekMessage());
            ShowMessages();
            DeleteMessages();
            Console.WriteLine("End Queues, hit enter for tables");
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
