using Microsoft.EntityFrameworkCore;
using MilitaryCollectiblesBackend.CustomClasses;
using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;

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
        Task<List<Insignia>> SearchInsignias(InsigniaSearchFilterDto filters);
        Task<List<Insignia>> SimpleSearch(string query);
    }
    public class InsigniasDataAccess : IInsigniasDataAccess
    {
        private readonly MilitaryCollectiblesDbContext _dbContext;

        public InsigniasDataAccess(MilitaryCollectiblesDbContext dbContext){
            _dbContext = dbContext;
        }

        public async Task<Insignia?> GetInsignia(int id){
            var insignia = await _dbContext.Insignias.FindAsync(id);
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
                throw new InvalidOperationException($"Insignia with name {insignia.Name} already exists.");
            }
            
            await _dbContext.Insignias.AddAsync(insignia);
            await _dbContext.SaveChangesAsync();
            return insignia;
        }

        public async Task<Insignia> UpdateInsignia(int id, Insignia insignia){
            var existingInsignia = await _dbContext.Insignias.FindAsync(id);

            if (existingInsignia == null)
            {
                throw new InvalidOperationException($"Insignia with ID {id} not found.");
            }

            insignia.Id = id; // Ensure the ID remains unchanged
            _dbContext.Entry(existingInsignia).CurrentValues.SetValues(insignia);
            await _dbContext.SaveChangesAsync();
            return existingInsignia;
        }

        //Handled by utilities controller after file upload
        public async Task UpdatePhotoUrl(int insigniaId, string photoUrl)
        {
            var insignia = await _dbContext.Insignias.FindAsync(insigniaId);
            if (insignia == null)
            {
                throw new InvalidOperationException($"Insignia with ID {insigniaId} not found.");
            }
            insignia.PhotoUrl = photoUrl;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteInsignia(int id){
            var exists = await _dbContext.Insignias.AnyAsync(i => i.Id == id);

            if (!exists)
            {
                throw new InvalidOperationException($"Insignia with ID {id} not found.");
            }

            await _dbContext.Insignias.Where(i => i.Id == id).ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            return;            
        }

        public async Task<List<Insignia>> GetInsigniaByPriceRange(decimal minPrice, decimal maxPrice){
                var results = await _dbContext.Insignias
                    .Where(i => i.Price >= minPrice && i.Price <= maxPrice)
                    .ToListAsync();
                return results;
        }

        public async Task<List<Insignia>> GetInsigniaByInsigniaType(string insigniaType){
            var results = await _dbContext.Insignias
                .Where(i => i.InsigniaType.InsigniaTypeName.ToLower() == insigniaType.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<Insignia>> GetInsigniaByOrigin(string origin)
        {
            var results = await _dbContext.Insignias
                .Where(i => i.Origin != null && i.Origin.OriginName.ToLower() == origin.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<Insignia>> GetInsigniaByEra(string era)
        {
            var results = await _dbContext.Insignias
                .Where(i => i.Era != null && i.Era.EraName.ToLower() == era.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<Insignia>> GetInsigniaByMaterial(string material)
        {
            var results = await _dbContext.Insignias
                .Where(i => i.Material != null && i.Material.MaterialName.ToLower() == material.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<Insignia>> GetInsigniaByPartOfSet(bool partOfSet)
        {    
            var results = await _dbContext.Insignias
                .Where(i => i.PartOfSet == partOfSet)
                .ToListAsync();

            return results;
        }

        public async Task<List<Insignia>> GetAllSeriesInsignias(int seriesId)
        {
            var exists = await _dbContext.QueriedInsigniaSeries.AnyAsync(s => s.SeriesId == seriesId);

            if (!exists)
            {
                throw new InvalidOperationException($"Insignia series with ID {seriesId} not found.");
            }

            var results = await _dbContext.Insignias
                .Where(i => i.SeriesId == seriesId)
                .ToListAsync();

            return results;
        }

        public async Task<List<Insignia>> GetInsigniasByStorageArea(int storageAreaId)
        {
            var exists = await _dbContext.StorageAreas.AnyAsync(s => s.Id == storageAreaId);

            if (!exists)
            {
                throw new InvalidOperationException($"Storage area with ID {storageAreaId} not found.");
            }

            var results = await _dbContext.Insignias
                .Where(i => i.StorageArea == storageAreaId)
                .ToListAsync();

            return results;            
        }

        public async Task UpdateAssignInsigniaToInsigniaSeries(int insigniaId, int seriesId){
            var insignia = await _dbContext.Insignias.FindAsync(insigniaId);
            if (insignia == null)
            {
                throw new InvalidOperationException($"Insignia with ID {insigniaId} not found.");
            }
            var seriesExists = await _dbContext.QueriedInsigniaSeries.AnyAsync(s => s.SeriesId == seriesId);
            if (!seriesExists)
            {
                throw new InvalidOperationException($"Insignia series with ID {seriesId} not found.");
            }

            insignia.SeriesId = seriesId;
            await _dbContext.SaveChangesAsync();
            return;
        }

        public async Task UpdateAssignInsigniaToStorageArea(int insigniaId, int storageAreaId)
        {
            var insignia = await _dbContext.Insignias.FindAsync(insigniaId);
            if (insignia == null)
            {
                throw new InvalidOperationException($"Insignia with ID {insigniaId} not found.");
            }

            var storageAreaExists = await _dbContext.StorageAreas.AnyAsync(s => s.Id == storageAreaId);
            if (!storageAreaExists)
            {
                throw new InvalidOperationException($"Storage area with ID {storageAreaId} not found.");
            }

            insignia.StorageArea = storageAreaId;
            await _dbContext.SaveChangesAsync();
            return;
        }

        public async Task<List<Insignia>> SearchInsignias(InsigniaSearchFilterDto filters)
        {
            var query = _dbContext.Insignias.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Name))
            {
                query = query.Where(i => i.Name.Contains(filters.Name));
            }
            if (filters.MinPrice.HasValue)
            {
                query = query.Where(i => i.Price >= filters.MinPrice.Value);
            }
            if (filters.MaxPrice.HasValue)
            {
                query = query.Where(i => i.Price <= filters.MaxPrice.Value);
            }
            if (!string.IsNullOrEmpty(filters.InsigniaType))
            {
                query = query.Where(i => i.InsigniaType.InsigniaTypeName.ToLower() == filters.InsigniaType.ToLower());
            }
            if (filters.PartOfSet.HasValue)
            {
                query = query.Where(i => i.PartOfSet == filters.PartOfSet.Value);
            }
            if (!string.IsNullOrEmpty(filters.Origin))
            {
                query = query.Where(i => i.Origin != null && i.Origin.OriginName.ToLower() == filters.Origin.ToLower());
            }
            if (!string.IsNullOrEmpty(filters.Era))
            {
                query = query.Where(i => i.Era != null && i.Era.EraName.ToLower() == filters.Era.ToLower());
            }
            if (!string.IsNullOrEmpty(filters.Material))
            {
                query = query.Where(i => i.Material != null && i.Material.MaterialName.ToLower() == filters.Material.ToLower());
            }

            return await query.ToListAsync();
        }
        
        public async Task<List<Insignia>> SimpleSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Insignia>();

            query = query.ToLower();

            var results = await _dbContext.Insignias
                .Where(i => i.Name.ToLower().Contains(query)
                // (i.Description != null && i.Description.ToLower().Contains(query)) need significant token matching for description field
                //(i.Origin != null && i.Origin.ToLower().Contains(query)) ||
                //(i.Era != null && i.Era.ToLower().Contains(query)) ||
                //(i.Material != null && i.Material.ToLower().Contains(query))
                )
                .ToListAsync();
            return results;
        }
    }
}
