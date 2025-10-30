using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface ILiteratureSeriesDataAccess
    {
        Task<LiteratureSeries?> GetLiteratureSeries(int id);
        Task<List<LiteratureSeries>> GetAllLiteratureSeries(int pageNumber, int pageSize);
        Task<LiteratureSeries> CreateLiteratureSeries(LiteratureSeries literatureSeries);
        Task<LiteratureSeries> UpdateLiteratureSeries(int id, LiteratureSeries literatureSeries);
        Task DeleteLiteratureSeries(int id);
    }
    public class LiteratureSeriesDataAccess : ILiteratureSeriesDataAccess
    {
        private readonly MilitaryCollectiblesDbContext _dbContext;

        public LiteratureSeriesDataAccess(MilitaryCollectiblesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LiteratureSeries?> GetLiteratureSeries(int id)
        {
            var literatureSeries = await _dbContext.QueriedLiteratureSeries.FindAsync(id);
            return literatureSeries;
        }

        public async Task<List<LiteratureSeries>> GetAllLiteratureSeries(int pageNumber, int pageSize)
        {
            var literatureSeriesList = await _dbContext.QueriedLiteratureSeries
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return literatureSeriesList;
        }

        public async Task<LiteratureSeries> CreateLiteratureSeries(LiteratureSeries literatureSeries)
        {
            var exists = await _dbContext.QueriedLiteratureSeries.AnyAsync(ls => ls.SeriesName == literatureSeries.SeriesName);
            if (exists)
            {
                throw new InvalidOperationException($"LiteratureSeries with name '{literatureSeries.SeriesName}' already exists.");
            }
            
            await _dbContext.QueriedLiteratureSeries.AddAsync(literatureSeries);
            await _dbContext.SaveChangesAsync();
            return literatureSeries;
        }

        public async Task<LiteratureSeries> UpdateLiteratureSeries(int id, LiteratureSeries literatureSeries)
        {
            var existingSeries = await _dbContext.QueriedLiteratureSeries.FindAsync(id);

            if (existingSeries == null)
            {
                throw new InvalidOperationException($"LiteratureSeries with ID {id} not found.");
            }

            literatureSeries.SeriesId = id; // Ensure the ID remains unchanged
            _dbContext.Entry(existingSeries).CurrentValues.SetValues(literatureSeries);
            await _dbContext.SaveChangesAsync();
            return existingSeries;
        }

        public async Task DeleteLiteratureSeries(int id)
        {
            var exists = await _dbContext.QueriedLiteratureSeries.AnyAsync(ls => ls.SeriesId == id);

            if (!exists)
            {
                throw new InvalidOperationException($"LiteratureSeries with ID {id} not found.");
            }

            await _dbContext.QueriedLiteratureSeries
                .Where(ls => ls.SeriesId == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            return;
        }
    }
}
