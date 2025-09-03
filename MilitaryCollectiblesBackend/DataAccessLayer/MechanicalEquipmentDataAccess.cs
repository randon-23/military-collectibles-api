using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface IMechanicalEquipmentDataAccess
    {
        Task<MechanicalEquipment?> GetMechanicalEquipment(int id);
        //public Task<IEnumerable<MechanicalEquipment>> GetAllMechanicalEquipments(); // This more flexible allows returning any collection type
        Task<List<MechanicalEquipment>> GetAllMechanicalEquipments(); // This for supporting indexing and collection operations, guarantees a list
        Task CreateMechanicalEquipment(MechanicalEquipment mechanicalEquipment);
        Task UpdateMechanicalEquipment(int id, MechanicalEquipment mechanicalEquipment);
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
    public class MechanicalEquipmentDataAccess
    {
    }
}
