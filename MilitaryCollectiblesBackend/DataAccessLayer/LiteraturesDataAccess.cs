using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface ILiteraturesDataAccess
    {
        Task<Literature?> GetLiterature(int id);
        //public Task<IEnumerable<Literature>> GetAllLiteratures(); // This more flexible allows returning any collection type
        Task<List<Literature>> GetAllLiteratures(int pageNumber, int pageSize); // This for supporting indexing and collection operations, guarantees a list
        Task<Literature> CreateLiterature(Literature literature);
        Task<Literature> UpdateLiterature(int id, Literature literature);
        Task DeleteLiterature(int id);

        //Task <List><Literature> GetLiteratureByAvailability - would this be useful?
        Task<List<Literature>> GetLiteratureByPriceRange(decimal minPrice, decimal maxPrice);
        Task<List<Literature>> GetLiteratureByAuthor(string author); // Would need to have way to get all available authors in the system, either by indexing all unique authors in Literatures table or by creating an Authors table
        Task<List<Literature>> GetLiteratureByPublicationYear(int publicationYear);
        Task<List<Literature>> GetLiteratureByPublicationYearRange(int startYear, int endYear);
        Task<List<Literature>> GetLiteratureByPublisher(string publisher); // Would need to have way to get all available publishers in the system, either by indexing all unique publishers in Literatures table or by creating a Publishers table
        Task<List<Literature>> GetLiteratureByISBN(string isbn); //string coz they have dashes
        Task<List<Literature>> GetLiteratureByLiteratureType(string literatureType); // Optionally could have a LiteratureTypes table to manage valid types, but currently i know what they are so just having the constraint good enough
        Task<List<Literature>> GetLiteratureByBindingType(string bindingType); // Optionally could have a BindingTypes table to manage valid types, but currently i know what they are so just having the constraint good enough
        Task<List<Literature>> GetAllSeriesLiteratures(int seriesId);
        // AddPhoto? - part of update equipment?
        Task<List<Literature>> GetLiteraturesByStorageArea(int storageAreaId);
        Task UpdateAssignLiteratureToLiteratureSeries(int literatureId, int seriesId);
        Task UpdateAssignLiteratureToStorageArea(int literatureId, int storageAreaId);
    }
    public class LiteraturesDataAccess : ILiteraturesDataAccess
    {
        private readonly MilitaryCollectiblesDbContext _dbContext;

        public LiteraturesDataAccess(MilitaryCollectiblesDbContext dbContext){
            _dbContext = dbContext;
        }

        public async Task<Literature?> GetLiterature(int id){
            var literature = await _dbContext.Literatures.FindAsync(id);
            if (literature == null)
            {
                throw new KeyNotFoundException($"Literature with ID {id} not found.");
            }
            return literature;
        }

        public async Task<List<Literature>> GetAllLiteratures(int pageNumber, int pageSize){
            var literatures = await _dbContext.Literatures
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return literatures;
        }

        public async Task<Literature> CreateLiterature(Literature literature){
            var exists = await _dbContext.Literatures.AnyAsync(l => l.Title == literature.Title);

            if (exists)
            {
                throw new Exception($"A literature record with the title {literature.Title} already exists.");
            }
            try
            {
                await _dbContext.Literatures.AddAsync(literature); //Async coz involves I/O operation
                await _dbContext.SaveChangesAsync();
                return literature;
            } catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while adding the literature to the database.", dbEx);
            }
        }

        public async Task<Literature> UpdateLiterature(int id, Literature literature){
            var existingLiterature = await _dbContext.Literatures.FindAsync(id);

            if (existingLiterature == null)
            {
                throw new KeyNotFoundException($"Literature with ID {id} not found.");
            }

            try
            {
                literature.Id = id; // Ensure the ID remains unchanged
                _dbContext.Entry(existingLiterature).CurrentValues.SetValues(literature); // does not need async coz modifies the tracked entity state 
                await _dbContext.SaveChangesAsync();
                return literature;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while updating the literature in the database.", dbEx);
            }
        }

        public async Task DeleteLiterature(int id){
            var exists = await _dbContext.Literatures
                .AnyAsync(l => l.Id == id);

            if (!exists)
            {
                throw new KeyNotFoundException($"Literature with ID {id} not found.");
            }

            try
            {
                await _dbContext.Literatures
                    .Where(l => l.Id == id)
                    .ExecuteDeleteAsync(); // EF Core 7.0+ feature for direct delete without loading entity
                await _dbContext.SaveChangesAsync();
                return;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the literature from the database.", dbEx);
            }
        }

        public async Task<List<Literature>> GetLiteratureByPriceRange(decimal minPrice, decimal maxPrice){
            try
            {
                if (minPrice < 0 || maxPrice < 0)
                {
                    throw new ArgumentException("Price values must be non-negative.");
                }
                if (minPrice > maxPrice)
                {
                    throw new ArgumentException("Minimum price cannot be greater than maximum price.");
                }

                var results = await _dbContext.Literatures
                    .Where(l => l.Price >= minPrice && l.Price <= maxPrice)
                    .ToListAsync();

                if(results.Count == 0 || results == null)
                {
                    return new List<Literature>();
                }

                return results;
            } catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    throw;
                }
                else
                {
                    throw new Exception("An error occurred while retrieving literatures by price range.", ex);
                }
            }
        }

        public async Task<List<Literature>> GetLiteratureByAuthor(string author){
            try
            {
                var results = await _dbContext.Literatures
                    .Where(l => l.Author != null && l.Author.ToLower() == author.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Literature>();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving literatures by author.", ex);
            }
        }

        public async Task<List<Literature>> GetLiteratureByPublicationYear(int publicationYear){
            try
            {
                var results = await _dbContext.Literatures
                    .Where(l => l.PublicationYear == publicationYear)
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Literature>();
                }
                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving literatures by publication year.", ex);
            }
        }

        public async Task<List<Literature>> GetLiteratureByPublicationYearRange(int startYear, int endYear){
            try
            {
                if (startYear > endYear)
                {
                    throw new ArgumentException("Start year cannot be greater than end year.");
                }
                if(startYear < 0 || endYear < 0)
                {
                    throw new ArgumentException("Year values must be non-negative.");
                }

                var results = await _dbContext.Literatures
                    .Where(l => l.PublicationYear >= startYear && l.PublicationYear <= endYear)
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Literature>();
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
                    throw new Exception("An error occurred while retrieving literatures by publication year range.", ex);
                }
            }
        }

        public async Task<List<Literature>> GetLiteratureByPublisher(string publisher){
            try
            {
                var results = await _dbContext.Literatures
                    .Where(l => l.Publisher != null && l.Publisher.ToLower() == publisher.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Literature>();
                }

                return results;
            } catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving literatures by publisher.", ex);
            }
        }

        public async Task<List<Literature>> GetLiteratureByISBN(string isbn){
            try
            {
                var results = await _dbContext.Literatures
                    .Where(l => l.ISBN != null && l.ISBN.ToLower() == isbn.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Literature>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving literatures by ISBN.", ex);
            }

        }

        public async Task<List<Literature>> GetLiteratureByLiteratureType(string literatureType){
            try
            {
                var results = await _dbContext.Literatures
                    .Where(l => l.LiteratureType.ToLower() == literatureType.ToLower())
                    .ToListAsync();
                
                if (results.Count == 0 || results == null)
                {
                    return new List<Literature>();
                }
                
                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving literatures by literature type.", ex);
            }
        }

        public async Task<List<Literature>> GetLiteratureByBindingType(string bindingType){
            try
            {
                var results = await _dbContext.Literatures
                    .Where(l => l.BindingType.ToLower() == bindingType.ToLower())
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Literature>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving literatures by binding type.", ex);
            }
        }

        public async Task<List<Literature>> GetAllSeriesLiteratures(int seriesId){
            try
            {
                var results = await _dbContext.Literatures
                    .Where(l => l.SeriesId == seriesId)
                    .ToListAsync();
            
                if (results.Count == 0 || results == null)
                {
                    return new List<Literature>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving literatures by series ID.", ex);
            }
        }

        public async Task<List<Literature>> GetLiteraturesByStorageArea(int storageAreaId){
            try
            {
                var results = await _dbContext.Literatures
                    .Where(l => l.StorageArea == storageAreaId)
                    .ToListAsync();

                if (results.Count == 0 || results == null)
                {
                    return new List<Literature>();
                }

                return results;
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving literatures by storage area ID.", ex);
            }
        }

        public async Task UpdateAssignLiteratureToLiteratureSeries(int literatureId, int seriesId){
            var literature = await _dbContext.Literatures.FindAsync(literatureId);
            if (literature == null)
            {
                throw new KeyNotFoundException($"Literature with ID {literatureId} not found.");
            }

            try
            {
                literature.SeriesId = seriesId;
                await _dbContext.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception("An error occurred while assigning literature to literature series.", ex);
            }
        }

        public async Task UpdateAssignLiteratureToStorageArea(int literatureId, int storageAreaId){
            var literature = await _dbContext.Literatures.FindAsync(literatureId);
            if (literature == null)
            {
                throw new KeyNotFoundException($"Literature with ID {literatureId} not found.");
            }
            try
            {
                literature.StorageArea = storageAreaId;
                await _dbContext.SaveChangesAsync();
            } catch (Exception ex)
            {
                throw new Exception("An error occurred while assigning literature to storage area.", ex);
            }
        }
    }
}
