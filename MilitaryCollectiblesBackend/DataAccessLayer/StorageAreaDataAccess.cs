using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface IStorageAreaDataAccess
    {
        Task<StorageArea?> GetStorageArea(int id);
        //public Task<IEnumerable<StorageArea>> GetAllStorageAreas(); // This more flexible allows
        Task<List<StorageArea>> GetAllStorageAreas(); // This for supporting indexing and collection operations, guarantees a list
        Task CreateStorageArea(StorageArea storageArea);
        Task UpdateStorageArea(int id, StorageArea storageArea);
        Task DeleteStorageArea(int id);
    }
    public class StorageAreaDataAccess
    {
    }
}
