using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureManager.TableEntities;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types

namespace AzureManager
{
    public class TableManager : ITableManager
    {
        private static CloudStorageAccount _storageAccount;
        private static CloudTableClient _tableClient;
        private static CloudTable _table;
        private static string _tableName;
        public TableManager(string tableName)
        {
            _tableName = tableName;
            _storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            _tableClient = _storageAccount.CreateCloudTableClient();
            CreateTableIfNotExists();
        }

        private void CreateTableIfNotExists()
        {
            // Retrieve a reference to a container.
            _table = _tableClient.GetTableReference(_tableName);

            // Create the queue if it doesn't already exist
            _table.CreateIfNotExists();
        }

        public void Write(MyEntity entity)
        {
            // Create the TableOperation object that inserts the entity.
            var operation = TableOperation.Insert(entity);

            // Execute the insert operation.
            _table.Execute(operation);
        }

        public void Update(MyEntity entity)
        {
            entity.ETag = "*";
            var operation = TableOperation.Replace(entity);
            _table.Execute(operation);
        }

        public void Delete(MyEntity entity)
        {
            var item = new MyEntity(Guid.Parse(entity.PartitionKey), entity.TrackId)
            {
                ETag = "*"
            };
            var operation=TableOperation.Delete(entity);
            _table.Execute(operation);
        }

        public void GetByTrackId(int trackId)
        {
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<MyEntity> query = new TableQuery<MyEntity>().Where(TableQuery.GenerateFilterCondition("TrackId", QueryComparisons.Equal, "5"));
            return query;
        }
    }
}
