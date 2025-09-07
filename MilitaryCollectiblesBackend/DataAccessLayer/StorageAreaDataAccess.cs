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
            if (storageArea == null)
            {
                throw new KeyNotFoundException($"StorageArea with ID {id} not found.");
            }
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
            var exists = await _dbContext.StorageAreas
                .AnyAsync(sa => sa.StorageAreaName == storageArea.StorageAreaName);
            if (exists)
            {
                throw new Exception($"LiteratureSeries with name '{storageArea.StorageAreaName}' already exists.");
            }
            try
            {
                await _dbContext.StorageAreas.AddAsync(storageArea);
                await _dbContext.SaveChangesAsync();
                return storageArea;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while creating the StorageArea.", ex);
            }
        }

        public async Task<StorageArea> UpdateStorageArea(int id, StorageArea storageArea)
        {
            var existingStorageArea = await _dbContext.StorageAreas.FindAsync(id);
            if (existingStorageArea == null)
            {
                throw new KeyNotFoundException($"StorageArea with ID {id} not found.");
            }

            try
            {
                storageArea.Id = id; // Ensure the ID remains unchanged
                _dbContext.Entry(existingStorageArea).CurrentValues.SetValues(storageArea);
                await _dbContext.SaveChangesAsync();
                return existingStorageArea;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the StorageArea.", ex);
            }
        }

        public async Task DeleteStorageArea(int id)
        {
            var exists = await _dbContext.StorageAreas.AnyAsync(sa => sa.Id == id);

            if (!exists)
            {
                throw new KeyNotFoundException($"StorageArea with ID {id} not found.");
            }

            try
            {
                await _dbContext.StorageAreas
                    .Where(sa => sa.Id == id)
                    .ExecuteDeleteAsync();
                await _dbContext.SaveChangesAsync();
                return;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the StorageArea.", ex);
            }
        }
    }
}
