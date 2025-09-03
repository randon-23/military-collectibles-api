using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface IEquipmentDataAccess
    {
        Task<Equipment?> GetEquipment(int id);
        //public Task<IEnumerable<Equipment>> GetAllEquipments(); // This more flexible allows returning any collection type
        Task<List<Equipment>> GetAllEquipments(); // This for supporting indexing and collection operations, guarantees a list
        Task CreateEquipment(Equipment equipment);
        Task UpdateEquipment(int id, Equipment equipment);
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
    public class EquipmentDataAccess
    {
    }
}
