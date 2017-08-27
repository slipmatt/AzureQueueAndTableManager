using AzureManager.TableEntities;

namespace AzureManager
{
    public interface ITableManager
    {
        void Delete(MyEntity entity);
        void GetByTrackId(int trackId);
        void Update(MyEntity entity);
        void Write(MyEntity entity);
    }
}