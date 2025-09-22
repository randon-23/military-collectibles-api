using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface IMechanicalEquipmentDataAccess
    {
        Task<MechanicalEquipment?> GetMechanicalEquipment(int id);
        //public Task<IEnumerable<MechanicalEquipment>> GetAllMechanicalEquipments(); // This more flexible allows returning any collection type
        Task<List<MechanicalEquipment>> GetAllMechanicalEquipments(int pageNumber, int pageSize); // This for supporting indexing and collection operations, guarantees a list
        Task<MechanicalEquipment> CreateMechanicalEquipment(MechanicalEquipment mechanicalEquipment);
        Task<MechanicalEquipment> UpdateMechanicalEquipment(int id, MechanicalEquipment mechanicalEquipment);
        Task UpdatePhotoUrl(int mechanicalEquipmentId, string photoUrl);
        Task DeleteMechanicalEquipment(int id);
        //Task <List><MechanicalEquipment> GetMechanicalEquipmentByAvailability - would this be useful?
        Task<List<MechanicalEquipment>> GetMechanicalEquipmentByPriceRange(decimal minPrice, decimal maxPrice);
        Task<List<MechanicalEquipment>> GetMechanicalEquipmentByType(string mechanicalEquipmentType); // Would need to have way to get all available mechanical equipment types in the system, either by indexing all unique mechanical equipment types in MechanicalEquipments table or by creating a MechanicalEquipmentTypes table
        Task<List<MechanicalEquipment>> GetMechanicalEquipmentByCaliberSpec(string caliberSpec); // Would need to have way to get all available caliber specs in the system, either by indexing all unique caliber specs in MechanicalEquipments table or by creating a CaliberSpecs table
        Task<List<MechanicalEquipment>> GetMechanicalEquipmentByVehicleModel(string vehicleModel); // Would need to have way to get all available vehicle models in the system, either by indexing all unique vehicle models in MechanicalEquipments table or by creating a VehicleModels table
        Task<List<MechanicalEquipment>> GetMechanicalEquipmentByManufacturer(string manufacturer); // Would need to have way to get all available manufacturers in the system, either by indexing all unique manufacturers in MechanicalEquipments table or by creating a Manufacturers table
        Task<List<MechanicalEquipment>> GetMechanicalEquipmentByOrigin(string origin); // Would need to have way to get all available origins in the system, either by indexing all unique origins in MechanicalEquipments table or by creating an Origins table
        Task<List<MechanicalEquipment>> GetMechanicalEquipmentByEra(string era); // Would need to have way to get all available eras in the system, either by indexing all unique eras in MechanicalEquipments table or by creating an Eras table
        Task<List<MechanicalEquipment>> GetMechanicalEquipmentByMaterial(string material); // Would need to have way to get all available materials in the system, either by indexing all unique materials in MechanicalEquipments table or by creating a Materials table
        Task<List<MechanicalEquipment>> GetMechanicalEquipmentsByStorageArea(int storageAreaId);
        Task UpdateAssignMechanicalEquipmentToStorageArea(int mechanicalEquipmentId, int storageAreaId);
    }
    public class MechanicalEquipmentDataAccess : IMechanicalEquipmentDataAccess
    {
        private readonly MilitaryCollectiblesDbContext _dbContext;

        public MechanicalEquipmentDataAccess(MilitaryCollectiblesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MechanicalEquipment?> GetMechanicalEquipment(int id)
        {
            var mechanicalEquipment = await _dbContext.MechanicalEquipments.FindAsync(id);
            if(mechanicalEquipment == null)
            {
                throw new KeyNotFoundException($"Mechanical Equipment with ID {id} not found.");
            }
            return mechanicalEquipment;
        }

        public async Task<List<MechanicalEquipment>> GetAllMechanicalEquipments(int pageNumber, int pageSize)
        {
            var mechanicalEquipments = await _dbContext.MechanicalEquipments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return mechanicalEquipments;
        }

        public async Task<MechanicalEquipment> CreateMechanicalEquipment(MechanicalEquipment mechanicalEquipment)
        {
            var exists = await _dbContext.MechanicalEquipments.AnyAsync(me => me.Name == mechanicalEquipment.Name);
            if (exists)
            {
                throw new Exception($"Mechanical Equipment with name {mechanicalEquipment.Name} already exists.");
            }

            try
            {
                await _dbContext.MechanicalEquipments.AddAsync(mechanicalEquipment);
                await _dbContext.SaveChangesAsync();
                return mechanicalEquipment;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while adding the mechanical equipment to the database.", dbEx);
            }
        }

        public async Task<MechanicalEquipment> UpdateMechanicalEquipment(int id, MechanicalEquipment mechanicalEquipment)
        {
            var existingMechanicalEquipment = await _dbContext.MechanicalEquipments.FindAsync(id);
            if (existingMechanicalEquipment == null)
            {
                throw new KeyNotFoundException($"Mechanical Equipment with ID {id} not found.");
            }

            try
            {
                mechanicalEquipment.Id = id; // Ensure the ID remains unchanged
                _dbContext.Entry(existingMechanicalEquipment).CurrentValues.SetValues(mechanicalEquipment);
                await _dbContext.SaveChangesAsync();
                return mechanicalEquipment;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while updating the mechanical equipment in the database.", dbEx);
            }
        }

        public async Task UpdatePhotoUrl(int mechanicalEquipmentId, string photoUrl)
        {
            var mechanicalEquipment = await _dbContext.MechanicalEquipments.FindAsync(mechanicalEquipmentId);
            if (mechanicalEquipment == null)
            {
                throw new KeyNotFoundException($"MechanicalEquipment with ID {mechanicalEquipmentId} not found.");
            }
            mechanicalEquipment.PhotoUrl = photoUrl;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteMechanicalEquipment(int id)
        {
            var exists = await _dbContext.MechanicalEquipments.AnyAsync(me => me.Id == id);

            if (!exists)
            {
                throw new KeyNotFoundException($"Mechanical Equipment with ID {id} not found.");
            }

            try
            {
                await _dbContext.MechanicalEquipments.Where(me => me.Id == id).ExecuteDeleteAsync();
                await _dbContext.SaveChangesAsync();
                return;
            } catch(DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the mechanical equipment from the database.", dbEx);
            }
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByPriceRange(decimal minPrice, decimal maxPrice)
        {
            try
            {
                if(minPrice < 0 || maxPrice < 0)
                {
                    throw new ArgumentException("Price values must be non-negative.");
                }
                if(minPrice > maxPrice)
                {
                    throw new ArgumentException("Minimum price cannot be greater than maximum price.");
                }

                var results = await _dbContext.MechanicalEquipments
                    .Where(me => me.Price >= minPrice && me.Price <= maxPrice)
                    .ToListAsync();

                if(results.Count == 0 || results == null)
                {
                    return new List<MechanicalEquipment>();
                }

                return results;
            } catch(Exception ex)
            {
                if(ex is ArgumentException)
                {
                    throw;
                }
                else
                {
                    throw new Exception("An error occurred while retrieving mechanical equipment by price range.", ex);
                }
            }
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByType(string mechanicalEquipmentType)
        {
            try
            {
                var results = await _dbContext.MechanicalEquipments
                    .Where(me => me.MechanicalEquipmentType.ToLower() == mechanicalEquipmentType.ToLower())
                    .ToListAsync();

                if(results.Count == 0 || results == null)
                {
                    return new List<MechanicalEquipment>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving mechanical equipment by type.", ex);
            }
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByCaliberSpec(string caliberSpec)
        {
            try
            {
                var results = await _dbContext.MechanicalEquipments
                    .Where(me => me.CaliberSpec != null && me.CaliberSpec.ToLower() == caliberSpec.ToLower())
                    .ToListAsync();

                if(results.Count == 0 || results == null)
                {
                    return new List<MechanicalEquipment>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving mechanical equipment by caliber spec.", ex);
            }
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByVehicleModel(string vehicleModel)
        {
            try
            {
                var results = await _dbContext.MechanicalEquipments
                    .Where(me => me.VehicleModel != null && me.VehicleModel.ToLower() == vehicleModel.ToLower())
                    .ToListAsync();

                if(results.Count == 0 || results == null)
                {
                    return new List<MechanicalEquipment>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving mechanical equipment by vehicle model.", ex);
            }
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByManufacturer(string manufacturer)
        {
            try
            {
                var results = await _dbContext.MechanicalEquipments
                    .Where(me => me.Manufacturer != null && me.Manufacturer.ToLower() == manufacturer.ToLower())
                    .ToListAsync();

                if(results.Count == 0 || results == null)
                {
                    return new List<MechanicalEquipment>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving mechanical equipment by manufacturer.", ex);
            }
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByOrigin(string origin)
        {
            try
            {
                var results = await _dbContext.MechanicalEquipments
                    .Where(me => me.Origin != null && me.Origin.ToLower() == origin.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<MechanicalEquipment>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving mechaincal equipment by origin.", ex);
            }
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByEra(string era)
        {
            try
            {
                var results = await _dbContext.MechanicalEquipments
                    .Where(me => me.Era != null && me.Era.ToLower() == era.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<MechanicalEquipment>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving mechanical equipment by origin.", ex);
            }
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByMaterial(string material)
        {
            try
            {
                var results = await _dbContext.MechanicalEquipments
                    .Where(me => me.Material != null && me.Material.ToLower() == material.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<MechanicalEquipment>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving mechanical equipment by material.", ex);
            }
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentsByStorageArea(int storageAreaId)
        {
            try
            {
                var results = await _dbContext.MechanicalEquipments
                    .Where(me => me.StorageArea == storageAreaId)
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<MechanicalEquipment>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving mechanical equipments by storage area ID.", ex);
            }
        }

        public async Task UpdateAssignMechanicalEquipmentToStorageArea(int mechanicalEquipmentId, int storageAreaId)
        {
            var mechanicalEquipment = await _dbContext.MechanicalEquipments.FindAsync(mechanicalEquipmentId);
            if (mechanicalEquipment == null)
            {
                throw new KeyNotFoundException($"Insignia with ID {mechanicalEquipmentId} not found.");
            }
            try
            {
                mechanicalEquipment.StorageArea = storageAreaId;
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while assigning the mechanical equipment to the storage area.", dbEx);
            }
        }
    }
}
