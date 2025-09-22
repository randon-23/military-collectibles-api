using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface IInsigniasDataAccess
    {
        Task<Insignia?> GetInsignia(int id);
        //public Task<IEnumerable<Insignia>> GetAllInsignias(); // This more flexible allows returning any collection type
        Task<List<Insignia>> GetAllInsignias(int pageNumber, int pageSize); // This for supporting indexing and collection operations, guarantees a list
        Task<Insignia> CreateInsignia(Insignia insignia);
        Task<Insignia> UpdateInsignia(int id, Insignia insignia);
        Task UpdatePhotoUrl(int insigniaId, string photoUrl);
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
    public class InsigniasDataAccess : IInsigniasDataAccess
    {
        private readonly MilitaryCollectiblesDbContext _dbContext;

        public InsigniasDataAccess(MilitaryCollectiblesDbContext dbContext){
            _dbContext = dbContext;
        }

        public async Task<Insignia?> GetInsignia(int id){
            var insignia = await _dbContext.Insignias.FindAsync(id);
            if(insignia == null)
            {
                throw new KeyNotFoundException($"Insignia with ID {id} not found.");
            }
            return insignia;
        }

        public async Task<List<Insignia>> GetAllInsignias(int pageNumber, int pageSize){
            var insignias = await _dbContext.Insignias
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return insignias;
        }

        public async Task<Insignia> CreateInsignia(Insignia insignia)
        {
            var exists = await _dbContext.Insignias.AnyAsync(i => i.Name == insignia.Name);

            if (exists)
            {
                throw new Exception($"Insignia with name {insignia.Name} already exists.");
            }
            try
            {
                await _dbContext.Insignias.AddAsync(insignia);
                await _dbContext.SaveChangesAsync();
                return insignia;
            } catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while creating the insignia.", dbEx);
            }
        }

        public async Task<Insignia> UpdateInsignia(int id, Insignia insignia){
            var existingInsignia = await _dbContext.Insignias.FindAsync(id);

            if (existingInsignia == null)
            {
                throw new KeyNotFoundException($"Insignia with ID {id} not found.");
            }

            try
            {
                insignia.Id = id; // Ensure the ID remains unchanged
                _dbContext.Entry(existingInsignia).CurrentValues.SetValues(insignia);
                await _dbContext.SaveChangesAsync();
                return existingInsignia;
            } catch(DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while updating the insignia.", dbEx);
            }
        }

        public async Task UpdatePhotoUrl(int insigniaId, string photoUrl)
        {
            var insignia = await _dbContext.Insignias.FindAsync(insigniaId);
            if (insignia == null)
            {
                throw new KeyNotFoundException($"Insignia with ID {insigniaId} not found.");
            }
            insignia.PhotoUrl = photoUrl;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteInsignia(int id){
            var exists = await _dbContext.Insignias.AnyAsync(i => i.Id == id);

            if (!exists)
            {
                throw new KeyNotFoundException($"Insignia with ID {id} not found.");
            }

            try
            {
                await _dbContext.Insignias.Where(i => i.Id == id).ExecuteDeleteAsync();
                await _dbContext.SaveChangesAsync();
                return;
            } catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the insignia.", dbEx);
            }
        }

        public async Task<List<Insignia>> GetInsigniaByPriceRange(decimal minPrice, decimal maxPrice){
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

                var results = await _dbContext.Insignias
                    .Where(i => i.Price >= minPrice && i.Price <= maxPrice)
                    .ToListAsync();

                if(results.Count == 0 || results == null)
                {
                    return new List<Insignia>();
                }

                return results;
            } catch(Exception ex)
            {
                if (ex is ArgumentException)
                {
                    throw;
                }
                else
                {
                    throw new Exception("An error occurred while retrieving insignias by price range.", ex);
                }
            }
        }

        public async Task<List<Insignia>> GetInsigniaByInsigniaType(string insigniaType){
            try
            {
                var results = await _dbContext.Insignias
                    .Where(i => i.InsigniaType.ToLower() == insigniaType.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Insignia>();
                }
                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving insignias by insignia type.", ex);
            }
        }

        public async Task<List<Insignia>> GetInsigniaByOrigin(string origin)
        {
            try
            {
                var results = await _dbContext.Insignias
                    .Where(i => i.Origin != null && i.Origin.ToLower() == origin.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Insignia>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving insignias by origin.", ex);
            }
        }

        public async Task<List<Insignia>> GetInsigniaByEra(string era)
        {
            try
            {
                var results = await _dbContext.Insignias
                    .Where(i => i.Era != null && i.Era.ToLower() == era.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Insignia>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving artifacts by origin.", ex);
            }
        }

        public async Task<List<Insignia>> GetInsigniaByMaterial(string material)
        {
            try
            {
                var results = await _dbContext.Insignias
                    .Where(i => i.Material != null && i.Material.ToLower() == material.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Insignia>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving insignias by material.", ex);
            }
        }

        public async Task<List<Insignia>> GetInsigniaByPartOfSet(bool partOfSet){
            try
            {
                var results = await _dbContext.Insignias
                    .Where(i => i.PartOfSet == partOfSet)
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Insignia>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving insignias by part of set.", ex);
            }
        }

        public async Task<List<Insignia>> GetAllSeriesInsignias(int seriesId){
            try
            {
                var results = await _dbContext.Insignias
                    .Where(i => i.SeriesId == seriesId)
                    .ToListAsync();

                if(results.Count == 0 || results == null)
                {
                    return new List<Insignia>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving insignias by series ID.", ex);
            }
        }

        public async Task<List<Insignia>> GetInsigniasByStorageArea(int storageAreaId){
            try
            {
                var results = await _dbContext.Insignias
                    .Where(i => i.StorageArea == storageAreaId)
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Insignia>();
                }

                return results;
            } catch (Exception ex){
                throw new Exception("An error occurred while retrieving insignias by storage area ID.", ex);
            }
        }

        public async Task UpdateAssignInsigniaToInsigniaSeries(int insigniaId, int seriesId){
            var insignia = await _dbContext.Insignias.FindAsync(insigniaId);
            if (insignia == null)
            {
                throw new KeyNotFoundException($"Insignia with ID {insigniaId} not found.");
            }

            try
            {
                insignia.SeriesId = seriesId;
                await _dbContext.SaveChangesAsync();
            } catch(DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while assigning the insignia to the series.", dbEx);
            }
        }

        public async Task UpdateAssignInsigniaToStorageArea(int insigniaId, int storageAreaId)
        {
            var insignia = await _dbContext.Insignias.FindAsync(insigniaId);
            if (insignia == null)
            {
                throw new KeyNotFoundException($"Insignia with ID {insigniaId} not found.");
            }
            try
            {
                insignia.StorageArea = storageAreaId;
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while assigning the insignia to the storage area.", dbEx);
            }
        }
    }
}
