using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface  IArtifactSeriesDataAccess
    {
        Task<ArtifactSeries> GetArtifactSeries(int id);
        Task<List<ArtifactSeries>> GetAllArtifactSeries();
        Task CreateArtifactSeries(ArtifactSeries artifactSeries);
        Task UpdateArtifactSeries(int id, ArtifactSeries artifactSeries);
        Task DeleteArtifactSeries(int id);
    }
    public class ArtifactSeriesDataAccess
    {
    }
}
