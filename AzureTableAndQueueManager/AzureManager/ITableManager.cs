using AzureManager.TableEntities;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureManager
{
    public interface ITableManager<T> where T : class
    {
        void Delete(T entity);
        TableQuery<T> Search(string field, int fieldValue);
        void Update(T entity);
        void Write(T entity);
    }
}