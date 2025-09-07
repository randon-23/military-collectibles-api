using Microsoft.EntityFrameworkCore;
using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface  IInsigniaSeriesDataAccess
    {
        Task<InsigniaSeries?> GetInsigniaSeries(int id);
        Task<List<InsigniaSeries>> GetAllInsigniaSeries(int pageNumber, int pageSize);
        Task<InsigniaSeries> CreateInsigniaSeries(InsigniaSeries insigniaSeries);
        Task<InsigniaSeries> UpdateInsigniaSeries(int id, InsigniaSeries insigniaSeries);
        Task DeleteInsigniaSeries(int id);
    }
    public class InsigniaSeriesDataAccess : IInsigniaSeriesDataAccess
    {
        private readonly MilitaryCollectiblesDbContext _dbContext;

        public InsigniaSeriesDataAccess(MilitaryCollectiblesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<InsigniaSeries?> GetInsigniaSeries(int id)
        {
            var insigniaSeries = await _dbContext.QueriedInsigniaSeries.FindAsync(id);
            if (insigniaSeries == null)
            {
                throw new KeyNotFoundException($"InsigniaSeries with ID {id} not found.");
            }
            return insigniaSeries;
        }

        public async Task<List<InsigniaSeries>> GetAllInsigniaSeries(int pageNumber, int pageSize)
        {
            var insigniaSeriesList = await _dbContext.QueriedInsigniaSeries
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return insigniaSeriesList;
        }

        public async Task<InsigniaSeries> CreateInsigniaSeries(InsigniaSeries insigniaSeries)
        {
            var exists = await _dbContext.QueriedInsigniaSeries
                .AnyAsync(ls => ls.SeriesName == insigniaSeries.SeriesName);
            if (exists)
            {
                throw new Exception($"InsigniaSeries with name '{insigniaSeries.SeriesName}' already exists.");
            }
            try
            {
                await _dbContext.QueriedInsigniaSeries.AddAsync(insigniaSeries);
                await _dbContext.SaveChangesAsync();
                return insigniaSeries;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while creating the InsigniaSeries.", ex);
            }
        }

        public async Task<InsigniaSeries> UpdateInsigniaSeries(int id, InsigniaSeries insigniaSeries)
        {
            var existingSeries = await _dbContext.QueriedInsigniaSeries.FindAsync(id);
            if (existingSeries == null)
            {
                throw new KeyNotFoundException($"InsigniaSeries with ID {id} not found.");
            }

            try
            {
                insigniaSeries.SeriesId = id; // Ensure the ID remains unchanged
                _dbContext.Entry(existingSeries).CurrentValues.SetValues(insigniaSeries);
                await _dbContext.SaveChangesAsync();
                return existingSeries;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the InsigniaSeries.", ex);
            }
        }

        public async Task DeleteInsigniaSeries(int id)
        {
            var exists = await _dbContext.QueriedInsigniaSeries.AnyAsync(ase => ase.SeriesId == id);

            if (!exists)
            {
                throw new KeyNotFoundException($"InsigniaSeries with ID {id} not found.");
            }

            try
            {
                await _dbContext.QueriedInsigniaSeries
                    .Where(ase => ase.SeriesId == id)
                    .ExecuteDeleteAsync();
                await _dbContext.SaveChangesAsync();
                return;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the InsigniaSeries.", ex);
            }
        }
    }
}
