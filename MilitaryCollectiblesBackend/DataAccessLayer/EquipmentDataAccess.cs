using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface IEquipmentDataAccess
    {
        Task<Equipment?> GetEquipment(int id);
        //public Task<IEnumerable<Equipment>> GetAllEquipments(); // This more flexible allows returning any collection type
        Task<List<Equipment>> GetAllEquipments(int pageNumber, int pageSize); // This for supporting indexing and collection operations, guarantees a list
        Task<Equipment> CreateEquipment(Equipment equipment);
        Task<Equipment> UpdateEquipment(int id, Equipment equipment);
        Task DeleteEquipment(int id);
        //Task <List><Equipment> GetEquipmentByAvailability - would this be useful?
        Task<List<Equipment>> GetEquipmentByPriceRange(decimal minPrice, decimal maxPrice);
        Task<List<Equipment>> GetEquipmentByEquipmentType(string equipmentType); // Would need to have way to get all available equipment types in the system, either by indexing all unique equipment types in Equipments table or by creating an EquipmentTypes table
        Task<List<Equipment>> GetEquipmentByOrigin(string origin); // Would need to have way to get all available origins in the system, either by indexing all unique origins in Equipments table or by creating an Origins table
        Task<List<Equipment>> GetEquipmentByEra(string era); // Would need to have way to get all available eras in the system, either by indexing all unique eras in Equipments table or by creating an Eras table
        Task<List<Equipment>> GetEquipmentByMaterial(string material); // Would need to have way to get all available materials in the system, either by indexing all unique materials in Equipments table or by creating a Materials table
        // AddPhoto? - part of update equipment?
        Task<List<Equipment>> GetEquipmentsByStorageArea(int storageAreaId);
        Task UpdateAssignEquipmentToStorageArea(int equipmentId, int storageAreaId);
    }
    public class EquipmentDataAccess : IEquipmentDataAccess
    {
        private readonly MilitaryCollectiblesDbContext _dbContext;

        public EquipmentDataAccess(MilitaryCollectiblesDbContext dbContext){
            _dbContext = dbContext;
        }

        public async Task<Equipment?> GetEquipment(int id){
            var equipment = await _dbContext.Equipments.FindAsync(id);
            if(equipment == null)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }
            return equipment;
        }

        public async Task<List<Equipment>> GetAllEquipments(int pageNumber, int pageSize){
            var equipments = await _dbContext.Equipments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return equipments;
        }

        public async Task<Equipment> CreateEquipment(Equipment equipment){
            var exists = await _dbContext.Equipments.AnyAsync(e => e.Name == equipment.Name);

            if (exists)
            {
                throw new Exception($"Equipment with name {equipment.Name} already exists.");
            }
            try
            {
                await _dbContext.Equipments.AddAsync(equipment);
                await _dbContext.SaveChangesAsync();
                return equipment;
            } catch(DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while creating the equipment.", dbEx);
            }
        }

        public async Task<Equipment> UpdateEquipment(int id, Equipment equipment){
            var existingEquipment = await _dbContext.Equipments.FindAsync(id);
            if (existingEquipment == null)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }

            try
            {
                equipment.Id = id; // Ensure the ID remains unchanged
                _dbContext.Entry(existingEquipment).CurrentValues.SetValues(equipment);
                await _dbContext.SaveChangesAsync();
                return existingEquipment;
            } catch(DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while updating the equipment.", dbEx);
            }
        }

        public async Task DeleteEquipment(int id){
            var exists = await _dbContext.Equipments.AnyAsync(e => e.Id == id);

            if (!exists)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }

            try
            {
                await _dbContext.Equipments.Where(e => e.Id == id).ExecuteDeleteAsync();
                await _dbContext.SaveChangesAsync();
                return;
            } catch(DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the equipment.", dbEx);
            }
        }

        public async Task<List<Equipment>> GetEquipmentByPriceRange(decimal minPrice, decimal maxPrice){
            try
            {
                if (minPrice < 0 || maxPrice < 0)
                {
                    throw new ArgumentException("Price values must be non-negative.");
                }
                if (minPrice > maxPrice)
                {
                    throw new ArgumentException("Minimum price cannot be greater than maximum price.");
                }

                var results = await _dbContext.Equipments
                    .Where(e => e.Price >= minPrice && e.Price <= maxPrice)
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Equipment>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving equipments by price range.", ex);
            }
        }

        public async Task<List<Equipment>> GetEquipmentByEquipmentType(string equipmentType)
        {
            try
            {
                var results = await _dbContext.Equipments
                    .Where(e => e.EquipmentType.ToLower() == equipmentType.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Equipment>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving equipments by equipment type.", ex);
            }
        }

        public async Task<List<Equipment>> GetEquipmentByOrigin(string origin)
        {
            try
            {
                var results = await _dbContext.Equipments
                    .Where(e => e.Origin != null && e.Origin.ToLower() == origin.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Equipment>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving equipments by origin.", ex);
            }
        }

        public async Task<List<Equipment>> GetEquipmentByEra(string era)
        {
            try
            {
                var results = await _dbContext.Equipments
                    .Where(e => e.Era != null && e.Era.ToLower() == era.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Equipment>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving equipments by origin.", ex);
            }
        }

        public async Task<List<Equipment>> GetEquipmentByMaterial(string material)
        {
            try
            {
                var results = await _dbContext.Equipments
                    .Where(e => e.Material != null && e.Material.ToLower() == material.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Equipment>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving equipments by material.", ex);
            }
        }

        public async Task<List<Equipment>> GetEquipmentsByStorageArea(int storageAreaId)
        {
            try
            {
                var results = await _dbContext.Equipments
                    .Where(e => e.StorageArea == storageAreaId)
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Equipment>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving equipments by storage area ID.", ex);
            }
        }

        public async Task UpdateAssignEquipmentToStorageArea(int equipmentId, int storageAreaId)
        {
            var equipment = await _dbContext.Equipments.FindAsync(equipmentId);
            if (equipment == null)
            {
                throw new KeyNotFoundException($"Insignia with ID {equipmentId} not found.");
            }
            try
            {
                equipment.StorageArea = storageAreaId;
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while assigning the equipment to the storage area.", dbEx);
            }
        }
    }
}
