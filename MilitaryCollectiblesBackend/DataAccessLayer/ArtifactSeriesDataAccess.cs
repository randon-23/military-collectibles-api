using Microsoft.EntityFrameworkCore;
using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface  IArtifactSeriesDataAccess
    {
        Task<ArtifactSeries?> GetArtifactSeries(int id);
        Task<List<ArtifactSeries>> GetAllArtifactSeries(int pageNumber, int pageSize);
        Task<ArtifactSeries> CreateArtifactSeries(ArtifactSeries artifactSeries);
        Task<ArtifactSeries> UpdateArtifactSeries(int id, ArtifactSeries artifactSeries);
        Task DeleteArtifactSeries(int id);
    }
    public class ArtifactSeriesDataAccess : IArtifactSeriesDataAccess
    {
        private readonly MilitaryCollectiblesDbContext _dbContext;

        public ArtifactSeriesDataAccess(MilitaryCollectiblesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ArtifactSeries?> GetArtifactSeries(int id)
        {
            var artifactSeries = await _dbContext.QueriedArtifactSeries.FindAsync(id);
            return artifactSeries;
        }

        public async Task<List<ArtifactSeries>> GetAllArtifactSeries(int pageNumber, int pageSize)
        {
            var artifactSeriesList = await _dbContext.QueriedArtifactSeries
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return artifactSeriesList;
        }

        public async Task<ArtifactSeries> CreateArtifactSeries(ArtifactSeries artifactSeries)
        {
            var exists = await _dbContext.QueriedArtifactSeries
                .AnyAsync(ase => ase.SeriesName == artifactSeries.SeriesName);
            if (exists)
            {
                throw new InvalidOperationException($"ArtifactSeries with name '{artifactSeries.SeriesName}' already exists.");
            }

            await _dbContext.QueriedArtifactSeries.AddAsync(artifactSeries);
            await _dbContext.SaveChangesAsync();
            return artifactSeries;
        }

        public async Task<ArtifactSeries> UpdateArtifactSeries(int id, ArtifactSeries artifactSeries)
        {
            var existingSeries = await _dbContext.QueriedArtifactSeries.FindAsync(id);
            if (existingSeries == null)
            {
                throw new InvalidOperationException($"ArtifactSeries with ID {id} not found.");
            }

            artifactSeries.SeriesId = id; // Ensure the ID remains unchanged
            _dbContext.Entry(existingSeries).CurrentValues.SetValues(artifactSeries);
            await _dbContext.SaveChangesAsync();
            return existingSeries;
        }

        public async Task DeleteArtifactSeries(int id)
        {
            var exists = await _dbContext.QueriedArtifactSeries.AnyAsync(ase => ase.SeriesId == id);

            if (!exists)
            {
                throw new InvalidOperationException($"ArtifactSeries with ID {id} not found.");
            }
                        
            await _dbContext.QueriedArtifactSeries
                .Where(ase => ase.SeriesId == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            return;
        }
    }
}
