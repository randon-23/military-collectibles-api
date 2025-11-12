using Microsoft.EntityFrameworkCore;
using MilitaryCollectiblesBackend.CustomClasses;
using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;

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
        Task<List<MechanicalEquipment>> SearchMechanicalEquipment(MechanicalEquipmentSearchFilterDto filters);
        Task<List<MechanicalEquipment>> SimpleSearch(string query);
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
                throw new InvalidOperationException($"Mechanical Equipment with name {mechanicalEquipment.Name} already exists.");
            }

            await _dbContext.MechanicalEquipments.AddAsync(mechanicalEquipment);
            await _dbContext.SaveChangesAsync();
            return mechanicalEquipment;
        }

        public async Task<MechanicalEquipment> UpdateMechanicalEquipment(int id, MechanicalEquipment mechanicalEquipment)
        {
            var existingMechanicalEquipment = await _dbContext.MechanicalEquipments.FindAsync(id);

            if (existingMechanicalEquipment == null)
            {
                throw new InvalidOperationException($"Mechanical Equipment with ID {id} not found.");
            }

            mechanicalEquipment.Id = id; // Ensure the ID remains unchanged
            _dbContext.Entry(existingMechanicalEquipment).CurrentValues.SetValues(mechanicalEquipment);
            await _dbContext.SaveChangesAsync();
            return mechanicalEquipment;
        }

        //Handled by utilities controller after file upload
        public async Task UpdatePhotoUrl(int mechanicalEquipmentId, string photoUrl)
        {
            var mechanicalEquipment = await _dbContext.MechanicalEquipments.FindAsync(mechanicalEquipmentId);
            if (mechanicalEquipment == null)
            {
                throw new InvalidOperationException($"MechanicalEquipment with ID {mechanicalEquipmentId} not found.");
            }
            mechanicalEquipment.PhotoUrl = photoUrl;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteMechanicalEquipment(int id)
        {
            var exists = await _dbContext.MechanicalEquipments.AnyAsync(me => me.Id == id);

            if (!exists)
            {
                throw new InvalidOperationException($"Mechanical Equipment with ID {id} not found.");
            }

            await _dbContext.MechanicalEquipments.Where(me => me.Id == id).ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            return;
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var results = await _dbContext.MechanicalEquipments
                .Where(me => me.Price >= minPrice && me.Price <= maxPrice)
                .ToListAsync();

            return results;
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByType(string mechanicalEquipmentType)
        {
            var results = await _dbContext.MechanicalEquipments
            .Where(me => me.MechanicalEquipmentType.MechanicalEquipmentTypeName.ToLower() == mechanicalEquipmentType.ToLower())
            .ToListAsync();

            return results;
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByCaliberSpec(string caliberSpec)
        {
            var exactMatch = await _dbContext.MechanicalEquipments
                .Where(me => me.CaliberSpec!= null && me.CaliberSpec.CaliberSpecName.ToLower() == caliberSpec.ToLower())
                .ToListAsync();

            if (exactMatch.Count > 0)
            {
                return exactMatch;
            }

            var similarMatches = await _dbContext.MechanicalEquipments
                .Where(me => me.CaliberSpec != null && me.CaliberSpec.CaliberSpecName.ToLower().Contains(caliberSpec.ToLower()))
                .ToListAsync();

            return similarMatches;
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByVehicleModel(string vehicleModel)
        {
            var exactMatch = await _dbContext.MechanicalEquipments
                .Where(me => me.VehicleModel != null && me.VehicleModel.ToLower() == vehicleModel.ToLower())
                .ToListAsync();

            if (exactMatch.Count > 0)
            {
                return exactMatch;
            }

            var similarMatches = await _dbContext.MechanicalEquipments
                .Where(me => me.VehicleModel != null && me.VehicleModel.ToLower().Contains(vehicleModel.ToLower()))
                .ToListAsync();

            return similarMatches;
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByManufacturer(string manufacturer)
        {
            var exactMatches = await _dbContext.MechanicalEquipments
                .Where(me => me.Manufacturer != null && me.Manufacturer.ManufacturerName.ToLower() == manufacturer.ToLower())
                .ToListAsync();

            if (exactMatches.Count > 0)
            {
                return exactMatches;
            }

            var similarMatches = await _dbContext.MechanicalEquipments
                .Where(me => me.Manufacturer != null && me.Manufacturer.ManufacturerName.Contains(manufacturer.ToLower()))
                .ToListAsync();

            return similarMatches;
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByOrigin(string origin)
        {
            var results = await _dbContext.MechanicalEquipments
                .Where(me => me.Origin != null && me.Origin.OriginName.ToLower() == origin.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByEra(string era)
        {
            var results = await _dbContext.MechanicalEquipments
                .Where(me => me.Era != null && me.Era.EraName.ToLower() == era.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentByMaterial(string material)
        {
            var results = await _dbContext.MechanicalEquipments
                .Where(me => me.Material != null && me.Material.MaterialName.ToLower() == material.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<MechanicalEquipment>> GetMechanicalEquipmentsByStorageArea(int storageAreaId)
        {
            var exists = await _dbContext.StorageAreas.AnyAsync(sa => sa.Id == storageAreaId);
            if (!exists)
            {
                throw new InvalidOperationException($"Storage area with ID {storageAreaId} not found.");
            }

            var results = await _dbContext.MechanicalEquipments
                .Where(me => me.StorageArea == storageAreaId)
                .ToListAsync();

            return results;
        }

        public async Task UpdateAssignMechanicalEquipmentToStorageArea(int mechanicalEquipmentId, int storageAreaId)
        {
            var mechanicalEquipment = await _dbContext.MechanicalEquipments.FindAsync(mechanicalEquipmentId);
            if (mechanicalEquipment == null)
            {
                throw new InvalidOperationException($"Mechanical equipment with ID {mechanicalEquipmentId} not found.");
            }

            var storageAreaExists = await _dbContext.StorageAreas.AnyAsync(sa => sa.Id == storageAreaId);
            if (!storageAreaExists)
            {
                throw new InvalidOperationException($"Storage area with ID {storageAreaId} not found.");
            }

            mechanicalEquipment.StorageArea = storageAreaId;
            await _dbContext.SaveChangesAsync();
            return;
        }

        public async Task<List<MechanicalEquipment>> SearchMechanicalEquipment(MechanicalEquipmentSearchFilterDto filters)
        {
            var query = _dbContext.MechanicalEquipments.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Name))
            {
                query = query.Where(m => m.Name.Contains(filters.Name));
            }
            if (filters.MinPrice.HasValue)
            {
                query = query.Where(m => m.Price >= filters.MinPrice.Value);
            }
            if (filters.MaxPrice.HasValue)
            {
                query = query.Where(m => m.Price <= filters.MaxPrice.Value);
            }
            if (!string.IsNullOrEmpty(filters.MechanicalEquipmentType))
            {
                query = query.Where(m => m.MechanicalEquipmentType.MechanicalEquipmentTypeName.ToLower() == filters.MechanicalEquipmentType.ToLower());
            }
            if (!string.IsNullOrEmpty(filters.CaliberSpec))
            {
                query = query.Where(m => m.CaliberSpec != null && m.CaliberSpec.CaliberSpecName.ToLower() == filters.CaliberSpec.ToLower());
            }
            if (!string.IsNullOrEmpty(filters.VehicleModel))
            {
                query = query.Where(m => m.VehicleModel != null && m.VehicleModel.ToLower() == filters.VehicleModel.ToLower());
            }
            if (!string.IsNullOrEmpty(filters.SerialNumber))
            {
                query = query.Where(m => m.SerialNumber != null && m.SerialNumber.ToLower() == filters.SerialNumber.ToLower());
            }
            if (!string.IsNullOrEmpty(filters.Manufacturer))
            {
                query = query.Where(m => m.Manufacturer != null && m.Manufacturer.ManufacturerName.ToLower() == filters.Manufacturer.ToLower());
            }
            if (!string.IsNullOrEmpty(filters.Origin))
            {
                query = query.Where(m => m.Origin != null && m.Origin.OriginName.ToLower() == filters.Origin.ToLower());
            }
            if (!string.IsNullOrEmpty(filters.Era))
            {
                query = query.Where(m => m.Era != null && m.Era.EraName.ToLower() == filters.Era.ToLower());
            }
            if (!string.IsNullOrEmpty(filters.Material))
            {
                query = query.Where(m => m.Material != null && m.Material.MaterialName.ToLower() == filters.Material.ToLower());
            }

            return await query.ToListAsync();
        }

        public async Task<List<MechanicalEquipment>> SimpleSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<MechanicalEquipment>();

            query = query.ToLower();
            var results = await _dbContext.MechanicalEquipments
                .Where(me => me.Name.ToLower().Contains(query) ||
                             //(me.CaliberSpec != null && me.CaliberSpec.ToLower().Contains(query)) ||
                             (me.VehicleModel != null && me.VehicleModel.ToLower().Contains(query)) ||
                             (me.SerialNumber != null && me.SerialNumber.ToLower().Contains(query)) ||
                             (me.Manufacturer != null && me.Manufacturer.ManufacturerName.ToLower().Contains(query))
                             //(me.Origin != null && me.Origin.ToLower().Contains(query)) ||
                             //(me.Era != null && me.Era.ToLower().Contains(query)) ||
                             //(me.Material != null && me.Material.ToLower().Contains(query)) ||
                             //(me.Description != null && me.Description.ToLower().Contains(query)) need significant token matching for description field
                      )
                .ToListAsync();
            return results;
        }
    }
}
