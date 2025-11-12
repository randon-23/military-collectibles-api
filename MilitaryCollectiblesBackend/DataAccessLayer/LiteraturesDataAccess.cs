using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using MilitaryCollectiblesBackend.CustomClasses;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface ILiteraturesDataAccess
    {
        Task<Literature?> GetLiterature(int id);
        //public Task<IEnumerable<Literature>> GetAllLiteratures(); // This more flexible allows returning any collection type
        Task<List<Literature>> GetAllLiteratures(int pageNumber, int pageSize); // This for supporting indexing and collection operations, guarantees a list
        Task<Literature> CreateLiterature(Literature literature);
        Task<Literature> UpdateLiterature(int id, Literature literature);
        Task UpdatePhotoUrl(int literatureId, string photoUrl);
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

        Task<List<Literature>> SearchLiterature(LiteratureSearchFilterDto filters);
        Task<List<Literature>> SimpleSearch(string query);
    }
    public class LiteraturesDataAccess : ILiteraturesDataAccess
    {
        private readonly MilitaryCollectiblesDbContext _dbContext;

        public LiteraturesDataAccess(MilitaryCollectiblesDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<Literature?> GetLiterature(int id) {
            var literature = await _dbContext.Literatures.FindAsync(id);
            return literature;
        }

        public async Task<List<Literature>> GetAllLiteratures(int pageNumber, int pageSize) {
            var literatures = await _dbContext.Literatures
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return literatures;
        }

        public async Task<Literature> CreateLiterature(Literature literature) {
            var exists = await _dbContext.Literatures.AnyAsync(l => l.Title == literature.Title);
            if (exists)
            {
                throw new InvalidOperationException($"A literature record with the title {literature.Title} already exists.");
            }

            await _dbContext.Literatures.AddAsync(literature); //Async coz involves I/O operation
            await _dbContext.SaveChangesAsync();
            return literature;
        }

        public async Task<Literature> UpdateLiterature(int id, Literature literature) {
            var existingLiterature = await _dbContext.Literatures.FindAsync(id);

            if (existingLiterature == null)
            {
                throw new InvalidOperationException($"Literature with ID {id} not found.");
            }

            literature.Id = id; // Ensure the ID remains unchanged
            _dbContext.Entry(existingLiterature).CurrentValues.SetValues(literature); // does not need async coz modifies the tracked entity state 
            await _dbContext.SaveChangesAsync();
            return existingLiterature;

        }

        //Handled by utilities controller after file upload
        public async Task UpdatePhotoUrl(int literatureId, string photoUrl)
        {
            var literature = await _dbContext.Literatures.FindAsync(literatureId);
            if (literature == null)
            {
                throw new InvalidOperationException($"Literature with ID {literatureId} not found.");
            }
            literature.PhotoUrl = photoUrl;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteLiterature(int id) {
            var exists = await _dbContext.Literatures.AnyAsync(l => l.Id == id);

            if (!exists)
            {
                throw new InvalidOperationException($"Literature with ID {id} not found.");
            }

            await _dbContext.Literatures
                .Where(l => l.Id == id)
                .ExecuteDeleteAsync(); // EF Core 7.0+ feature for direct delete without loading entity
            await _dbContext.SaveChangesAsync();
            return;
        }

        public async Task<List<Literature>> GetLiteratureByPriceRange(decimal minPrice, decimal maxPrice) {
            var results = await _dbContext.Literatures
                .Where(l => l.Price >= minPrice && l.Price <= maxPrice)
                .ToListAsync();

            return results;
        }

        public async Task<List<Literature>> GetLiteratureByAuthor(string author) {
            var results = await _dbContext.Literatures
                .Where(l => l.Author != null && l.Author.AuthorName.ToLower() == author.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<Literature>> GetLiteratureByPublicationYear(int publicationYear) {
            var results = await _dbContext.Literatures
                .Where(l => l.PublicationYear == publicationYear)
                .ToListAsync();

            return results;
        }

        public async Task<List<Literature>> GetLiteratureByPublicationYearRange(int startYear, int endYear) {
            var results = await _dbContext.Literatures
                .Where(l => l.PublicationYear >= startYear && l.PublicationYear <= endYear)
                .ToListAsync();

            return results;
        }

        public async Task<List<Literature>> GetLiteratureByPublisher(string publisher) {
            var results = await _dbContext.Literatures
                .Where(l => l.Publisher != null && l.Publisher.PublisherName.ToLower() == publisher.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<Literature>> GetLiteratureByISBN(string isbn) {
            var results = await _dbContext.Literatures
                .Where(l => l.ISBN != null && l.ISBN.ToLower() == isbn.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<Literature>> GetLiteratureByLiteratureType(string literatureType) {
            var results = await _dbContext.Literatures
                .Where(l => l.LiteratureType.LiteratureTypeName.ToLower() == literatureType.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<Literature>> GetLiteratureByBindingType(string bindingType) {
            var results = await _dbContext.Literatures
                .Where(l => l.BindingType.BindingTypeName.ToLower() == bindingType.ToLower())
                .ToListAsync();

            return results;
        }

        public async Task<List<Literature>> GetAllSeriesLiteratures(int seriesId) {
            var exists = await _dbContext.QueriedLiteratureSeries.AnyAsync(s => s.SeriesId == seriesId);

            if (!exists)
            {
                throw new InvalidOperationException($"Literature series with ID {seriesId} not found.");
            }

            var results = await _dbContext.Literatures
                .Where(l => l.SeriesId == seriesId)
                .ToListAsync();

            return results;
        }

        public async Task<List<Literature>> GetLiteraturesByStorageArea(int storageAreaId) {
            var exists = await _dbContext.StorageAreas.AnyAsync(s => s.Id == storageAreaId);

            if (!exists)
            {
                throw new InvalidOperationException($"Storage area with ID {storageAreaId} not found.");
            }

            var results = await _dbContext.Literatures
                .Where(l => l.StorageArea == storageAreaId)
                .ToListAsync();

            return results;
        }

        public async Task UpdateAssignLiteratureToLiteratureSeries(int literatureId, int seriesId) {
            var literature = await _dbContext.Literatures.FindAsync(literatureId);
            if (literature == null)
            {
                throw new InvalidOperationException($"Literature with ID {literatureId} not found.");
            }

            var seriesExists = await _dbContext.QueriedLiteratureSeries.AnyAsync(s => s.SeriesId == seriesId);
            if (!seriesExists)
            {
                throw new InvalidOperationException($"Literature series with ID {seriesId} not found.");
            }

            literature.SeriesId = seriesId;
            await _dbContext.SaveChangesAsync();

            return;
        }

        public async Task UpdateAssignLiteratureToStorageArea(int literatureId, int storageAreaId) {
            var literature = await _dbContext.Literatures.FindAsync(literatureId);
            if (literature == null)
            {
                throw new InvalidOperationException($"Literature with ID {literatureId} not found.");
            }

            var storageAreaExists = await _dbContext.StorageAreas.AnyAsync(s => s.Id == storageAreaId);
            if (!storageAreaExists)
            {
                throw new InvalidOperationException($"Storage area with ID {storageAreaId} not found.");
            }

            literature.StorageArea = storageAreaId;
            await _dbContext.SaveChangesAsync();

            return;
        }

        public async Task<List<Literature>> SearchLiterature(LiteratureSearchFilterDto filters)
        {
            var query = _dbContext.Literatures.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Title))
            {
                query = query.Where(l => l.Title.Contains(filters.Title));
            }
            if (!string.IsNullOrEmpty(filters.Author))
            {
                var authorLower = filters.Author.ToLower();
                query = query.Where(l => l.Author != null && l.Author.AuthorName.ToLower() == authorLower);
            }
            if (filters.MinPrice.HasValue)
            {
                query = query.Where(l => l.Price >= filters.MinPrice.Value);
            }
            if (filters.MaxPrice.HasValue)
            {
                query = query.Where(l => l.Price <= filters.MaxPrice.Value);
            }
            if (filters.YearPublished.HasValue)
            {
                query = query.Where(l => l.PublicationYear == filters.YearPublished.Value);
            }
            if (filters.PublicationYearFrom.HasValue)
            {
                query = query.Where(l => l.PublicationYear >= filters.PublicationYearFrom.Value);
            }
            if (filters.PublicationYearTo.HasValue)
            {
                query = query.Where(l => l.PublicationYear <= filters.PublicationYearTo.Value);
            }
            if (!string.IsNullOrEmpty(filters.Publisher))
            {
                var publisherLower = filters.Publisher.ToLower();
                query = query.Where(l => l.Publisher != null && l.Publisher.PublisherName.ToLower() == publisherLower);
            }
            if (!string.IsNullOrEmpty(filters.ISBN))
            {
                var isbnLower = filters.ISBN.ToLower();
                query = query.Where(l => l.ISBN != null && l.ISBN.ToLower() == isbnLower);
            }
            if (!string.IsNullOrEmpty(filters.LiteratureType))
            {
                var typeLower = filters.LiteratureType.ToLower();
                query = query.Where(l => l.LiteratureType.LiteratureTypeName.ToLower() == typeLower);
            }
            if (!string.IsNullOrEmpty(filters.BindingType))
            {
                var bindingLower = filters.BindingType.ToLower();
                query = query.Where(l => l.BindingType.BindingTypeName.ToLower() == bindingLower);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Literature>> SimpleSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Literature>();

            query = query.ToLower();

            var results = await _dbContext.Literatures
                .Where(l =>
                    (l.Title != null && l.Title.ToLower().Contains(query)) ||
                    // (l.Description != null && l.Description.ToLower().Contains(query)) || need significant token matching for description field
                    (l.Author != null && l.Author.AuthorName.ToLower().Contains(query)) ||
                    (l.Publisher != null && l.Publisher.PublisherName.ToLower().Contains(query)) ||
                    (l.ISBN != null && l.ISBN.ToLower().Contains(query))
                ).ToListAsync();

            return results;
        }
    }
}
