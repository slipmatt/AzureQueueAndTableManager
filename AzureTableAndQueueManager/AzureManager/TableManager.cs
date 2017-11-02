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
    public class TableManager<T> : ITableManager<T> where T : class
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

        public void Write(T entity)
        {

            // Create the TableOperation object that inserts the entity.

            var operation = TableOperation.Insert((ITableEntity) entity);

            // Execute the insert operation.
            _table.Execute(operation);
        }

        public void Update(T entity)
        {
            var thisEntity = (ITableEntity) entity;
            thisEntity.ETag = "*";
            var operation = TableOperation.Replace(thisEntity);
            _table.Execute(operation);
        }

        public void Delete(T entity)
        {
            var thisEntity = (ITableEntity)entity;
            thisEntity.ETag = "*";
            var operation=TableOperation.Delete(thisEntity);
            _table.Execute(operation);
        }

         TableQuery<T> ITableManager<T>.Search(string field, int fieldValue)
        {
            
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition(field, QueryComparisons.Equal, fieldValue.ToString()));
          // var result = _table.ExecuteQuery(query).ToList();
            return query;
        }
    }
}
