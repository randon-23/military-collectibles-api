using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface IArtifactsDataAccess
    {
        Task<Artifact?> GetArtifact(int id);
        //public Task<IEnumerable<Artifact>> GetAllArtifacts(); // This more flexible allows returning any collection type
        Task<List<Artifact>> GetAllArtifacts(int pageNumber, int pageSize); // This for supporting indexing and collection operations, guarantees a list    
        Task <Artifact> CreateArtifact(Artifact artifact);
        Task <Artifact> UpdateArtifact(int id, Artifact artifact);
        Task UpdatePhotoUrl(int artifactId, string photoUrl);
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
    public class ArtifactsDataAccess : IArtifactsDataAccess
    {
        private readonly MilitaryCollectiblesDbContext _dbContext;

        public ArtifactsDataAccess(MilitaryCollectiblesDbContext dbContext){
            _dbContext = dbContext;
        }

        public async Task<Artifact?> GetArtifact(int id){
            var artifact = await _dbContext.Artifacts.FindAsync(id);
            return artifact;
        }
        public async Task<List<Artifact>> GetAllArtifacts(int pageNumber, int pageSize) {
            var artifacts = await _dbContext.Artifacts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return artifacts;

        }
        public async Task<Artifact> CreateArtifact(Artifact artifact) {
            var exists = await _dbContext.Artifacts.AnyAsync(a => a.Name == artifact.Name);

            if (exists)
            {
                throw new InvalidOperationException($"An artifact with this name already exists.");
            }

            await _dbContext.Artifacts.AddAsync(artifact);
            await _dbContext.SaveChangesAsync();
            return artifact;
        }
        public async Task<Artifact> UpdateArtifact(int id, Artifact artifact) {
            var existingArtifact = await _dbContext.Artifacts.FindAsync(id);

            if (existingArtifact == null)
            {
                throw new InvalidOperationException($"Artifact with ID {id} not found.");
            }

            artifact.Id = id;
            _dbContext.Entry(existingArtifact).CurrentValues.SetValues(artifact);
            await _dbContext.SaveChangesAsync();
            return existingArtifact;
        }

        //Handled by utilities controller after file upload
        public async Task UpdatePhotoUrl(int artifactId, string photoUrl)
        {
            var artifact = await _dbContext.Artifacts.FindAsync(artifactId);
            if (artifact == null)
            {
                throw new InvalidOperationException($"Artifact with ID {artifactId} not found.");
            }
            artifact.PhotoUrl = photoUrl;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteArtifact(int id) {
            var exists = await _dbContext.Artifacts.AnyAsync(a => a.Id == id);

            if (!exists)
            {
                throw new InvalidOperationException($"Artifact with ID {id} not found.");
            }

            await _dbContext.Artifacts.Where(a => a.Id == id).ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            return;
        }
        public async Task<List<Artifact>> GetArtifactByPriceRange(decimal minPrice, decimal maxPrice) {
            var results = await _dbContext.Artifacts
                .Where(a => a.Price >= minPrice && a.Price <= maxPrice)
                .ToListAsync();

            return results;
        }

        public async Task<List<Artifact>> GetArtifactByArtifactType(string artifactType) {
            var results = await _dbContext.Artifacts
                .Where(a => a.ArtifactType.ToLower() == artifactType.ToLower())
                .ToListAsync();

            return results;
        }
        public async Task<List<Artifact>> GetArtifactByOrigin(string origin) {
            var results = await _dbContext.Artifacts
                .Where(a => a.Origin != null && a.Origin.ToLower() == origin.ToLower())
                .ToListAsync();

            return results;
        }
        public async Task<List<Artifact>> GetArtifactByEra(string era) {
            var results = await _dbContext.Artifacts
                .Where(a => a.Era != null && a.Era.ToLower() == era.ToLower())
                .ToListAsync();

            return results;
             
        }
        public async Task<List<Artifact>> GetAllSeriesArtifacts(int seriesId) {
            var exists = await _dbContext.QueriedArtifactSeries.AnyAsync(s => s.SeriesId == seriesId);

            if (!exists)
            {
                throw new InvalidOperationException($"Artifact series with ID {seriesId} not found.");
            }

            var results = await _dbContext.Artifacts
                .Where(a => a.SeriesId == seriesId)
                .ToListAsync();

            return results;
        }
        public async Task<List<Artifact>> GetArtifactsByStorageArea(int storageAreaId) {
            var exists = await _dbContext.StorageAreas.AnyAsync(s => s.Id == storageAreaId);
            if (!exists)
            {
                throw new InvalidOperationException($"Storage area with ID {storageAreaId} not found.");
            }

            var results = await _dbContext.Artifacts
                .Where(a => a.StorageArea == storageAreaId)
                .ToListAsync();

            return results;
        }
        public async Task UpdateAssignArtifactToArtifactSeries(int artifactId, int seriesId) {
            var artifact = await _dbContext.Artifacts.FindAsync(artifactId);
            if (artifact == null)
            {
                throw new InvalidOperationException($"Artifact with ID {artifactId} not found.");
            }

            var seriesExists = await _dbContext.QueriedArtifactSeries.AnyAsync(s => s.SeriesId == seriesId);
            if (!seriesExists)
            {
                throw new InvalidOperationException($"Artifact series with ID {seriesId} not found.");
            }
                        
            artifact.SeriesId = seriesId;
            await _dbContext.SaveChangesAsync();

            return;
        }
        public async Task UpdateAssignArtifactToStorageArea(int artifactId, int storageAreaId)
        {
            var artifact = await _dbContext.Artifacts.FindAsync(artifactId);
            if (artifact == null)
            {
                throw new InvalidOperationException($"Artifact with ID {artifactId} not found.");
            }

            var storageAreaExists = await _dbContext.StorageAreas.AnyAsync(s => s.Id == storageAreaId);
            if (!storageAreaExists)
            {
                throw new InvalidOperationException($"Storage area with ID {storageAreaId} not found.");
            }

            artifact.StorageArea = storageAreaId;
            await _dbContext.SaveChangesAsync();

            return;
        }
    }
}
