using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface IInsigniasDataAccess
    {
        Task<Insignia?> GetInsignia(int id);
        //public Task<IEnumerable<Insignia>> GetAllInsignias(); // This more flexible allows returning any collection type
        Task<List<Insignia>> GetAllInsignias(); // This for supporting indexing and collection operations, guarantees a list
        Task CreatesInsignia(Insignia insignia);
        Task UpdateInsignia(int id, Insignia insignia);
        Task DeleteInsignia(int id);

        //Task <List><Insignia> GetInsigniaByAvailability - would this be useful?
        Task<List<Insignia>> GetInsigniaByPriceRange(decimal minPrice, decimal maxPrice);
        Task<List<Insignia>> GetInsigniaByInsigniaType(string insigniaType); // Would need to have way to get all available insignia types in the system, either by indexing all unique insignia types in Insignias table or by creating an InsigniaTypes table
        Task<List<Insignia>> GetInsigniaByOrigin(string origin); // Would need to have way to get all available origins in the system, either by indexing all unique origins in Insignias table or by creating an Origins table
        Task<List<Insignia>> GetInsigniaByEra(string era); // Would need to have way to get all available eras in the system, either by indexing all unique eras in Insignias table or by creating an Eras table
        Task<List<Insignia>> GetInsigniaByMaterial(string material); // Would need to have way to get all available materials in the system, either by indexing all unique materials in Insignias table or by creating a Materials table
        Task<List<Insignia>> GetInsigniaByPartOfSet(bool partOfSet);
        Task<List<Insignia>> GetAllSeriesInsignias(int seriesId);
        // AddPhoto? - part of update equipment?
        Task<List<Insignia>> GetInsigniasByStorageArea(int storageAreaId);
        Task UpdateAssignInsigniaToInsigniaSeries(int insigniaId, int seriesId);
        Task UpdateAssignInsigniaToStorageArea(int insigniaId, int storageAreaId);
    }
    public class InsigniasDataAccess
    {
    }
}
