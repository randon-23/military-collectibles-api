using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilitaryCollectiblesBackend.CustomClasses;
using MilitaryCollectiblesBackend.DataAccessLayer;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    // Inject Required Services per Controller: Only the services needed for the controller’s responsibility (e.g., photo update for utilities, search/data access for search).
    // Note: Can have advanced filtering functionality which interfaces directly with DAL and then next to it client can also filter by only one, which would call the controller method.
    // To support direct attribute-based queries on a single entity type
    {
        private readonly ILiteraturesDataAccess _literaturesDataAccess;
        private readonly IArtifactsDataAccess _artifactsDataAccess;
        private readonly IInsigniasDataAccess _insigniasDataAccess;
        private readonly IEquipmentDataAccess _equipmentDataAccess;
        private readonly IMechanicalEquipmentDataAccess _mechanicalEquipmentDataAccess;

        public SearchController(
                ILiteraturesDataAccess literaturesDataAccess,
                IArtifactsDataAccess artifactsDataAccess,
                IInsigniasDataAccess insigniasDataAccess,
                IEquipmentDataAccess equipmentDataAccess,
                IMechanicalEquipmentDataAccess mechanicalEquipmentDataAccess
            )
        {
            _literaturesDataAccess = literaturesDataAccess;
            _artifactsDataAccess = artifactsDataAccess;
            _insigniasDataAccess = insigniasDataAccess;
            _equipmentDataAccess = equipmentDataAccess;
            _mechanicalEquipmentDataAccess = mechanicalEquipmentDataAccess;
        }

        // posts since complex search with multiple parameters to include in body instead of query string (not RESTful as GET but more practical)

        // CURRENTLY considering entity-specific DTOs for search parameters to avoid bloated method signatures. 
        // Each DTO would encapsulate relevant search criteria for its entity type meaning each entity would have its own endpoint.
        // In future, could consider a more generic search DTO with optional parameters and an entity type specifier for a unified search endpoint.

        [HttpPost("search-literatures")]
        public async Task<ActionResult<List<Literature>>> SearchLiteratures([FromBody]LiteratureSearchFilterDto filters)
        {
            try
            {
                // Validate price range if provided
                if (filters.MinPrice.HasValue && filters.MinPrice < 0)
                    return BadRequest("MinPrice must be non-negative.");
                if (filters.MaxPrice.HasValue && filters.MaxPrice < 0)
                    return BadRequest("MaxPrice must be non-negative.");
                if (filters.MinPrice.HasValue && filters.MaxPrice.HasValue && filters.MinPrice > filters.MaxPrice)
                    return BadRequest("MinPrice cannot be greater than MaxPrice.");

                // Validate publication year ranges
                var currentYear = DateTime.Now.Year;
                if (filters.YearPublished.HasValue && (filters.YearPublished < 0 || filters.YearPublished > currentYear))
                    return BadRequest($"YearPublished must be between 0 and {currentYear}.");
                if (filters.PublicationYearFrom.HasValue && (filters.PublicationYearFrom < 0 || filters.PublicationYearFrom > currentYear))
                    return BadRequest($"PublicationYearFrom must be between 0 and {currentYear}.");
                if (filters.PublicationYearTo.HasValue && (filters.PublicationYearTo < 0 || filters.PublicationYearTo > currentYear))
                    return BadRequest($"PublicationYearTo must be between 0 and {currentYear}.");
                if (filters.PublicationYearFrom.HasValue && filters.PublicationYearTo.HasValue && filters.PublicationYearFrom > filters.PublicationYearTo)
                    return BadRequest("PublicationYearFrom cannot be greater than PublicationYearTo.");

                
                var result = await _literaturesDataAccess.SearchLiterature(filters);

                if (result == null || result.Count == 0)
                {
                    return NotFound("No literatures found matching the criteria.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("search-artifacts")]
        public async Task<ActionResult<List<Artifact>>> SearchArtifacts([FromBody]ArtifactSearchFilterDto filters)
        {
            try
            {
                // Validate price ranges if provided
                if (filters.MinPrice.HasValue && filters.MinPrice < 0)
                    return BadRequest("MinPrice must be non-negative.");
                if (filters.MaxPrice.HasValue && filters.MaxPrice < 0)
                    return BadRequest("MaxPrice must be non-negative.");
                if (filters.MinPrice.HasValue && filters.MaxPrice.HasValue && filters.MinPrice > filters.MaxPrice)
                    return BadRequest("MinPrice cannot be greater than MaxPrice.");

                var artifacts = await _artifactsDataAccess.SearchArtifacts(filters);

                if (artifacts == null || artifacts.Count == 0)
                    return NotFound("No artifacts found matching the criteria.");

                return Ok(artifacts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    
        [HttpPost("search-insignias")]
        public async Task<ActionResult<List<Insignia>>> SearchInsignias([FromBody] InsigniaSearchFilterDto filters)
        {
            try
            {
                if (filters.MinPrice.HasValue && filters.MinPrice < 0)
                    return BadRequest("MinPrice must be non-negative.");
                if (filters.MaxPrice.HasValue && filters.MaxPrice < 0)
                    return BadRequest("MaxPrice must be non-negative.");
                if (filters.MinPrice.HasValue && filters.MaxPrice.HasValue && filters.MinPrice > filters.MaxPrice)
                    return BadRequest("MinPrice cannot be greater than MaxPrice.");

                var insignias = await _insigniasDataAccess.SearchInsignias(filters);

                if (insignias == null || insignias.Count == 0)
                    return NotFound("No insignias found matching the criteria.");

                return Ok(insignias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("search-equipment")]
        public async Task<ActionResult<List<Equipment>>> SearchEquipment([FromBody] EquipmentSearchFilterDto filters)
        {
            try
            {
                if (filters.MinPrice.HasValue && filters.MinPrice < 0)
                    return BadRequest("MinPrice must be non-negative.");
                if (filters.MaxPrice.HasValue && filters.MaxPrice < 0)
                    return BadRequest("MaxPrice must be non-negative.");
                if (filters.MinPrice.HasValue && filters.MaxPrice.HasValue && filters.MinPrice > filters.MaxPrice)
                    return BadRequest("MinPrice cannot be greater than MaxPrice.");

                var equipmentList = await _equipmentDataAccess.SearchEquipment(filters);

                if (equipmentList == null || equipmentList.Count == 0)
                    return NotFound("No equipment found matching the criteria.");

                return Ok(equipmentList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("search-mechanical-equipment")]
        public async Task<ActionResult<List<MechanicalEquipment>>> SearchMechanicalEquipment([FromBody] MechanicalEquipmentSearchFilterDto filters)
        {
            try
            {
                if (filters.MinPrice.HasValue && filters.MinPrice < 0)
                    return BadRequest("MinPrice must be non-negative.");
                if (filters.MaxPrice.HasValue && filters.MaxPrice < 0)
                    return BadRequest("MaxPrice must be non-negative.");
                if (filters.MinPrice.HasValue && filters.MaxPrice.HasValue && filters.MinPrice > filters.MaxPrice)
                    return BadRequest("MinPrice cannot be greater than MaxPrice.");

                var results = await _mechanicalEquipmentDataAccess.SearchMechanicalEquipment(filters);

                if (results == null || results.Count == 0)
                    return NotFound("No mechanical equipment found matching the criteria.");

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("search/{query}")]
        public async Task<ActionResult<dynamic>> SearchBar([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return BadRequest("Query must be provided.");

                var literatureResults = await _literaturesDataAccess.SimpleSearch(query);
                var artifactResults = await _artifactsDataAccess.SimpleSearch(query);
                var insigniaResults = await _insigniasDataAccess.SimpleSearch(query);
                var equipmentResults = await _equipmentDataAccess.SimpleSearch(query);
                var mechanicalEquipmentResults = await _mechanicalEquipmentDataAccess.SimpleSearch(query);

                // Combine results in a container object or flat list with entity tagging
                var combinedResults = new
                {
                    Literatures = literatureResults,
                    Artifacts = artifactResults,
                    Insignias = insigniaResults,
                    Equipment = equipmentResults,
                    MechanicalEquipment = mechanicalEquipmentResults
                };

                return Ok(combinedResults);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
