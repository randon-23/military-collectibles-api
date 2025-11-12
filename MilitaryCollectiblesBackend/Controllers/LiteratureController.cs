using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilitaryCollectiblesBackend.CustomClasses;
using MilitaryCollectiblesBackend.DataAccessLayer;
using MilitaryCollectiblesBackend.Models;
using System.Text.RegularExpressions;

namespace MilitaryCollectiblesBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiteratureController : ControllerBase
    {
        private readonly ILiteraturesDataAccess _literaturesDataAccess;
        public LiteratureController(ILiteraturesDataAccess literaturesDataAccess)
        {
            _literaturesDataAccess = literaturesDataAccess;
        }

        [HttpGet("get-literature/{id}")]
        public async Task<ActionResult<Literature>> GetLiterature(int id)
        {
            try
            {
                var literature = await _literaturesDataAccess.GetLiterature(id);
                if (literature == null)
                {
                    return NotFound($"Literature with ID {id} not found.");
                }
                return Ok(literature);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-literatures")]
        public async Task<ActionResult<List<Literature>>> GetAllLiteratures([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            try
            {
                var literatures = await _literaturesDataAccess.GetAllLiteratures(pageNumber, pageSize);
                if (literatures == null || literatures.Count == 0)
                {
                    return NotFound("No literatures found.");
                }

                return Ok(literatures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpPost("create-literature")]
        public async Task<ActionResult<ResponseDto<Literature>>> CreateLiterature([FromBody] Literature literature)
        {
            if (literature.Title.IsNullOrEmpty() || literature.Description.IsNullOrEmpty() || literature.LiteratureType.LiteratureTypeName.IsNullOrEmpty() || literature.BindingType.BindingTypeName.IsNullOrEmpty())
            {
                return BadRequest("Title, Description, LiteratureType, and BindingType are required fields and cannot be empty.");
            }

            if (!literature.Availability)
            {
                return BadRequest("Literature must be available at point of creation.");
            }

            if (literature.Price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }

            if (literature.PublicationYear.HasValue)
            {
                var currentYear = DateTime.Now.Year;
                if (literature.PublicationYear > currentYear)
                {
                    return BadRequest($"Invalid Publication Year - must be before {currentYear}.");
                }
            }

            // For rest of fields, will create tables for each which will have predefined values to select from, and will validate against those tables.
            // But that should be done in DAL methods.

            // For storage area and series, will validate in DAL methods as well.

            // When adding photo URL, need to figure out how to handle that - will likely use a cloud storage solution and save the URL here.
            try
            {
                var createdLiterature = await _literaturesDataAccess.CreateLiterature(literature);
                var response = new ResponseDto<Literature> { CreatedObject = createdLiterature, entityType = "literature" };
                return CreatedAtAction(nameof(GetLiterature), new { id = createdLiterature.Id }, response); // Return 201 with URL to get the newly created literature with the Id and the created object
                // The 3 parameters are: the name of the action, route values (in this case the id of the created literature), and the created object itself.
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpPut("update-literature/{id}")]
        public async Task<ActionResult<ResponseDto<Literature>>> UpdateLiterature(int id, [FromBody] Literature literature)
        {
            // Basic validation, similar to create
            if (literature.Title.IsNullOrEmpty() || literature.Description.IsNullOrEmpty() ||
               literature.LiteratureType.LiteratureTypeName.IsNullOrEmpty() || literature.BindingType.BindingTypeName.IsNullOrEmpty())
            {
                return BadRequest("Title, Description, LiteratureType, and BindingType are required fields and cannot be empty.");
            }
            if (literature.Price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }
            if (literature.PublicationYear.HasValue)
            {
                var currentYear = DateTime.Now.Year;
                if (literature.PublicationYear > currentYear)
                {
                    return BadRequest($"Invalid Publication Year - must be before {currentYear}.");
                }
            }

            try
            {
                var updatedLiterature = await _literaturesDataAccess.UpdateLiterature(id, literature);
                var response = new ResponseDto<Literature> { CreatedObject = updatedLiterature, entityType = "literature" };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete-literature/{id}")]
        public async Task<ActionResult> DeleteLiterature(int id)
        {
            try
            {
                await _literaturesDataAccess.DeleteLiterature(id);
                return NoContent(); // 204 status code for successful deletion with no content
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-literature-by-price-range")]
        public async Task<ActionResult<List<Literature>>> GetLiteratureByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            try
            {
                if (minPrice < 0 || maxPrice < 0)
                {
                    BadRequest("Price values must be non-negative.");
                }
                if (minPrice > maxPrice)
                {
                    BadRequest("Minimum price cannot be greater than maximum price.");
                }

                var literatures = await _literaturesDataAccess.GetLiteratureByPriceRange(minPrice, maxPrice);
                if (literatures == null || literatures.Count == 0)
                {
                    return NotFound("No literatures found in the specified price range.");
                }
                return Ok(literatures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-literature-by-author/{author}")]
        public async Task<ActionResult<List<Literature>>> GetLiteratureByAuthor(string author)
        {
            try
            {
                var literatures = await _literaturesDataAccess.GetLiteratureByAuthor(author);
                if(literatures == null || literatures.Count == 0)
                {
                    return NotFound($"No literatures found by author: {author}");
                }
                return Ok(literatures);
            } catch(Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-literature-by-pub-year/{year}")]
        public async Task<ActionResult<List<Literature>>> GetLiteratureByPublicationYear(int year)
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                if (year < 0 || year > currentYear)
                {
                    return BadRequest($"Invalid Publication Year - must be between 0 and {currentYear}.");
                }

                var literatures = await _literaturesDataAccess.GetLiteratureByPublicationYear(year);
                if (literatures == null || literatures.Count == 0)
                {
                    return NotFound($"No literatures found published in the year: {year}");
                }
                return Ok(literatures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-literature-by-pub-year-range")]
        public async Task<ActionResult<List<Literature>>> GetLiteratureByPublicationYearRange([FromQuery] int startYear, [FromQuery] int endYear)
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                if (startYear < 0 || endYear < 0 || startYear > currentYear || endYear > currentYear)
                {
                    return BadRequest($"Invalid Publication Year - must be between 0 and {currentYear}.");
                }
                if (startYear > endYear)
                {
                    return BadRequest("Start year cannot be greater than end year.");
                }
                var literatures = await _literaturesDataAccess.GetLiteratureByPublicationYearRange(startYear, endYear);
                if (literatures == null || literatures.Count == 0)
                {
                    return NotFound($"No literatures found published between the years: {startYear} and {endYear}");
                }
                return Ok(literatures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-literature-by-publisher/{publisher}")]
        public async Task<ActionResult<List<Literature>>> GetLiteratureByPublisher(string publisher)
        {
            try
            {
                var literatures = await _literaturesDataAccess.GetLiteratureByPublisher(publisher);
                if (literatures == null || literatures.Count == 0)
                {
                    return NotFound($"No literatures found by publisher: {publisher}");
                }

                return Ok(literatures);
            } catch(Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-literature-by-isbn/{isbn}")]
        public async Task<ActionResult<List<Literature>>> GetLiteratureByISBN(string isbn)
        {
            try
            {
                string isbnPattern = @"^(97(8|9))?\d{9}(\d|X)$";
                var cleanedIsbn = isbn.Replace("-", "").Replace(" ", "");
                if (!Regex.IsMatch(cleanedIsbn, isbnPattern, RegexOptions.IgnoreCase))
                {
                    return BadRequest("Invalid ISBN format. Provide a valid ISBN-10 or ISBN-13.");
                }

                var literatures = await _literaturesDataAccess.GetLiteratureByISBN(isbn);
                if(literatures == null || literatures.Count == 0)
                {
                    return NotFound($"No litreatures found by ISBN: {isbn}");
                }

                return Ok(literatures);
            } catch(Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-literature-by-literature-type/{literatureType}")]
        public async Task<ActionResult<List<Literature>>> GetLiteratureByLiteratureType(string literatureType)
        {
            try
            {
                // Validation against predefined types will be done in DAL method
                var literatures = await _literaturesDataAccess.GetLiteratureByLiteratureType(literatureType);
                if (literatures == null || literatures.Count == 0)
                {
                    return NotFound($"No literatures found of type: {literatureType}");
                }

                return Ok(literatures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-litreature-by-binding-type/{bindingType}")]
        public async Task<ActionResult<List<Literature>>> GetLiteratureByBindingType(string bindingType)
        {
            try
            {
                // Validation against predefined types will be done in DAL method
                var literatures = await _literaturesDataAccess.GetLiteratureByBindingType(bindingType);
                if (literatures == null || literatures.Count == 0)
                {
                    return NotFound($"No literatures found with binding type: {bindingType}");
                }

                return Ok(literatures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all-series-literatures/{seriesId}")]
        public async Task<ActionResult<List<Literature>>> GetAllSeriesLiteratures(int seriesId)
        {
            try
            {
                var literatures = await _literaturesDataAccess.GetAllSeriesLiteratures(seriesId);
                if (literatures == null || literatures.Count == 0)
                {
                    return NotFound($"No literatures found for series ID: {seriesId}");
                }

                return Ok(literatures);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-literature-by-storage-area/{storageAreaId}")]
        public async Task<ActionResult<List<Literature>>> GetLiteraturesByStorageArea(int storageAreaId)
        {
            try
            {
                var literatures = await _literaturesDataAccess.GetLiteraturesByStorageArea(storageAreaId);
                if (literatures == null || literatures.Count == 0)
                {
                    return NotFound($"No literatures found in storage area ID: {storageAreaId}");
                }

                return Ok(literatures);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpPut("assign-literature-to-series/{literatureId}/series/{seriesId}")]
        public async Task<ActionResult> UpdateAssignLiteratureToLiteratureSeries(int literatureId, int seriesId)
        {
            try
            {
                await _literaturesDataAccess.UpdateAssignLiteratureToLiteratureSeries(literatureId, seriesId);
                return NoContent();
            } 
            catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }

        [HttpPut("assign-literature-to-storage-area/{literatureId}/storage-area/{storageAreaId}")]
        public async Task<ActionResult> UpdateAssignLiteratureToStorageArea(int literatureId, int storageAreaId)
        {
            try
            {
                await _literaturesDataAccess.UpdateAssignLiteratureToStorageArea(literatureId, storageAreaId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An  error occurred: {ex.Message}");
            }
        }
    }
}
