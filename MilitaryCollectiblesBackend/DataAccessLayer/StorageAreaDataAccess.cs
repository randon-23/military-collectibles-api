using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface IStorageAreaDataAccess
    {
        Task<StorageArea?> GetStorageArea(int id);
        //public Task<IEnumerable<StorageArea>> GetAllStorageAreas(); // This more flexible allows
        Task<List<StorageArea>> GetAllStorageAreas(int pageNumber, int pageSize); // This for supporting indexing and collection operations, guarantees a list
        Task<StorageArea> CreateStorageArea(StorageArea storageArea);
        Task<StorageArea> UpdateStorageArea(int id, StorageArea storageArea);
        Task DeleteStorageArea(int id);
    }
    public class StorageAreaDataAccess : IStorageAreaDataAccess
    {
        private readonly MilitaryCollectiblesDbContext _dbContext;

        public StorageAreaDataAccess(MilitaryCollectiblesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<StorageArea?> GetStorageArea(int id)
        {
            var storageArea = await _dbContext.StorageAreas.FindAsync(id);
            return storageArea;
        }

        public async Task<List<StorageArea>> GetAllStorageAreas(int pageNumber, int pageSize)
        {
            var storageAreaList = await _dbContext.StorageAreas
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return storageAreaList;
        }

        public async Task<StorageArea> CreateStorageArea(StorageArea storageArea)
        {
            var exists = await _dbContext.StorageAreas.AnyAsync(sa => sa.StorageAreaName == storageArea.StorageAreaName);
            if (exists)
            {
                throw new InvalidOperationException($"LiteratureSeries with name '{storageArea.StorageAreaName}' already exists.");
            }
            
            await _dbContext.StorageAreas.AddAsync(storageArea);
            await _dbContext.SaveChangesAsync();
            return storageArea;
        }

        public async Task<StorageArea> UpdateStorageArea(int id, StorageArea storageArea)
        {
            var existingStorageArea = await _dbContext.StorageAreas.FindAsync(id);
            if (existingStorageArea == null)
            {
                throw new InvalidOperationException($"StorageArea with ID {id} not found.");
            }

            storageArea.Id = id; // Ensure the ID remains unchanged
            _dbContext.Entry(existingStorageArea).CurrentValues.SetValues(storageArea);
            await _dbContext.SaveChangesAsync();
            return existingStorageArea;
        }

        public async Task DeleteStorageArea(int id)
        {
            var exists = await _dbContext.StorageAreas.AnyAsync(sa => sa.Id == id);
            if (!exists)
            {
                throw new InvalidOperationException($"StorageArea with ID {id} not found.");
            }

            await _dbContext.StorageAreas
                .Where(sa => sa.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            return;
        }
    }
}
