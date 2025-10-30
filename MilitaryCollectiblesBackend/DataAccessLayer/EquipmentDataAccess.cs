﻿using MilitaryCollectiblesBackend.Data;
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
        Task UpdatePhotoUrl(int equipmentId, string photoUrl);
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
                throw new InvalidOperationException($"An equipment with name {equipment.Name} already exists.");
            }
            await _dbContext.Equipments.AddAsync(equipment);
            await _dbContext.SaveChangesAsync();
            return equipment;
        }

        public async Task<Equipment> UpdateEquipment(int id, Equipment equipment){
            var existingEquipment = await _dbContext.Equipments.FindAsync(id);
            if (existingEquipment == null)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }

            equipment.Id = id; // Ensure the ID remains unchanged
            _dbContext.Entry(existingEquipment).CurrentValues.SetValues(equipment);
            await _dbContext.SaveChangesAsync();
            return existingEquipment;
        }

        //Handled by utilities controller after file upload
        public async Task UpdatePhotoUrl(int equipmentId, string photoUrl)
        {
            var equipment = await _dbContext.Equipments.FindAsync(equipmentId);
            if (equipment == null)
            {
                throw new InvalidOperationException($"Equipment with ID {equipmentId} not found.");
            }
            equipment.PhotoUrl = photoUrl;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEquipment(int id){
            var exists = await _dbContext.Equipments.AnyAsync(e => e.Id == id);

            if (!exists)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }

            await _dbContext.Equipments.Where(e => e.Id == id).ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            return;
        }

        public async Task<List<Equipment>> GetEquipmentByPriceRange(decimal minPrice, decimal maxPrice){
            var results = await _dbContext.Equipments
                .Where(e => e.Price >= minPrice && e.Price <= maxPrice)
                .ToListAsync();

            return results;            
        }

        public async Task<List<Equipment>> GetEquipmentByEquipmentType(string equipmentType)
        {
            var results = await _dbContext.Equipments
                .Where(e => e.EquipmentType.ToLower() == equipmentType.ToLower())
                .ToListAsync();

            return results;   
        }

        public async Task<List<Equipment>> GetEquipmentByOrigin(string origin)
        {
            var results = await _dbContext.Equipments
                .Where(e => e.Origin != null && e.Origin.ToLower() == origin.ToLower())
                .ToListAsync();

            return results;   
        }

        public async Task<List<Equipment>> GetEquipmentByEra(string era)
        {
            var results = await _dbContext.Equipments
                .Where(e => e.Era != null && e.Era.ToLower() == era.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<Equipment>> GetEquipmentByMaterial(string material)
        {
            var results = await _dbContext.Equipments
                .Where(e => e.Material != null && e.Material.ToLower() == material.ToLower())
                .ToListAsync();

            return results; 
        }

        public async Task<List<Equipment>> GetEquipmentsByStorageArea(int storageAreaId)
        {
            var exists = await _dbContext.StorageAreas.AnyAsync(sa => sa.Id == storageAreaId);
            if (!exists)
            {
                throw new InvalidOperationException($"Storage area with ID {storageAreaId} not found.");
            }

            var results = await _dbContext.Equipments
                .Where(e => e.StorageArea == storageAreaId)
                .ToListAsync();

            return results;
        }

        public async Task UpdateAssignEquipmentToStorageArea(int equipmentId, int storageAreaId)
        {
            var equipment = await _dbContext.Equipments.FindAsync(equipmentId);
            if (equipment == null)
            {
                throw new InvalidOperationException($"Insignia with ID {equipmentId} not found.");
            }
            
            var storageAreaExists = await _dbContext.StorageAreas.AnyAsync(sa => sa.Id == storageAreaId);
            if (!storageAreaExists)
            {
                throw new InvalidOperationException($"Storage area with ID {storageAreaId} not found.");
            }

            equipment.StorageArea = storageAreaId;
            await _dbContext.SaveChangesAsync();

            return;
        }
    }
}
