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
                throw new InvalidOperationException($"InsigniaSeries with name '{insigniaSeries.SeriesName}' already exists.");
            }
            
            await _dbContext.QueriedInsigniaSeries.AddAsync(insigniaSeries);
            await _dbContext.SaveChangesAsync();
            return insigniaSeries;
        }

        public async Task<InsigniaSeries> UpdateInsigniaSeries(int id, InsigniaSeries insigniaSeries)
        {
            var existingSeries = await _dbContext.QueriedInsigniaSeries.FindAsync(id);
            if (existingSeries == null)
            {
                throw new InvalidOperationException($"InsigniaSeries with ID {id} not found.");
            }

            insigniaSeries.SeriesId = id; // Ensure the ID remains unchanged
            _dbContext.Entry(existingSeries).CurrentValues.SetValues(insigniaSeries);
            await _dbContext.SaveChangesAsync();
            return existingSeries;
        }

        public async Task DeleteInsigniaSeries(int id)
        {
            var exists = await _dbContext.QueriedInsigniaSeries.AnyAsync(ase => ase.SeriesId == id);

            if (!exists)
            {
                throw new InvalidOperationException($"InsigniaSeries with ID {id} not found.");
            }

            await _dbContext.QueriedInsigniaSeries
                .Where(ase => ase.SeriesId == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            return;
        }
    }
}
