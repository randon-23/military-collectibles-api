using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface IArtifactsDataAccess
    {
        Task<Artifact?> GetArtifact(int id);
        //public Task<IEnumerable<Artifact>> GetAllArtifacts(); // This more flexible allows returning any collection type
        Task<List<Artifact>> GetAllArtifacts(); // This for supporting indexing and collection operations, guarantees a list    
        Task CreateArtifact(Artifact artifact);
        Task UpdateArtifact(int id, Artifact artifact);
        Task DeleteArtifact(int id);
        //Task <List><Artifact> GetArtifactByAvailability - would this be useful?
        Task<List<Artifact>> GetArtifactByPriceRange(decimal minPrice, decimal maxPrice);
        Task<List<Artifact>> GetArtifactByArtifactType(string artifactType); // Would need to have way to get all available artifact types in the system, either by indexing all unique artifact types in Artifacts table or by creating an ArtifactTypes table
        Task<List<Artifact>> GetArtifactByOrigin(string origin); // Would need to have way to get all available origins in the system, either by indexing all unique origins in Artifacts table or by creating an Origins table
        Task<List<Artifact>> GetArtifactByEra(string era); // Would need to have way to get all available eras in the system, either by indexing all unique eras in Artifacts table or by creating an Eras table
        Task<List<Artifact>> GetAllSeriesArtifacts(int seriesId);
        // AddPhoto? - part of update equipment?
        Task<List<Artifact>> GetArtifactsByStorageArea(int storageAreaId);
        Task UpdateAssignArtifactToArtifactSeries(int artifactId, int seriesId);
        Task UpdateAssignArtifactToStorageArea(int artifactId, int storageAreaId);
    }
    public class ArtifactsDataAccess
    {
    }
}
