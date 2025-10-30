using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilitaryCollectiblesBackend.CustomClasses;
using MilitaryCollectiblesBackend.DataAccessLayer;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsigniaController : ControllerBase
    {
        private readonly IInsigniasDataAccess _insigniasDataAccess;

        public InsigniaController(IInsigniasDataAccess insigniasDataAccess)
        {
            _insigniasDataAccess = insigniasDataAccess;
        }

        [HttpGet("get-insignia/{id}")]
        public async Task<ActionResult<Insignia>> GetInsignia(int id)
        {
            try
            {
                var insignia = await _insigniasDataAccess.GetInsignia(id);
                if (insignia == null)
                {
                    return NotFound($"Insignia with ID {id} not found.");
                }

                return Ok(insignia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all-insignias")]
        public async Task<ActionResult<List<Insignia>>> GetAllInsignias([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            try
            {
                var insignias = await _insigniasDataAccess.GetAllInsignias(pageNumber, pageSize);
                if (insignias == null || insignias.Count == 0)
                {
                    return NotFound("No insignias found.");
                }

                return Ok(insignias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("create-insignia")]
        public async Task<ActionResult<ResponseDto<Insignia>>> CreateInsignia([FromBody] Insignia insignia)
        {
            if (insignia.Name.IsNullOrEmpty() || insignia.Description.IsNullOrEmpty() || insignia.InsigniaType.IsNullOrEmpty())
            {
                return BadRequest("Name, Description, and InsigniaType are required fields.");
            }
            if (!insignia.Availability)
            {
                return BadRequest("New insignia must be marked as available.");
            }
            if (insignia.Price < 0)
            {
                return BadRequest("Price must be a non-negative value.");
            }

            try
            {
                var createdInsignia = await _insigniasDataAccess.CreateInsignia(insignia);
                var response = new ResponseDto<Insignia> { CreatedObject = createdInsignia, entityType = "insignia" };
                return CreatedAtAction(nameof(GetInsignia), new { id = createdInsignia.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update-insignia/{id}")]
        public async Task<ActionResult<Insignia>> UpdateInsignia(int id, [FromBody] Insignia insignia)
        {
            if (insignia.Name.IsNullOrEmpty() || insignia.Description.IsNullOrEmpty() || insignia.InsigniaType.IsNullOrEmpty())
            {
                return BadRequest("Name, Description, and InsigniaType are required fields.");
            }
            if (insignia.Price < 0)
            {
                return BadRequest("Price must be a non-negative value.");
            }

            try
            {
                var updatedInsignia = await _insigniasDataAccess.UpdateInsignia(id, insignia);
                var response = new ResponseDto<Insignia> { CreatedObject = updatedInsignia, entityType = "insignia" };
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete-insignia/{id}")]
        public async Task<ActionResult> DeleteInsignia(int id)
        {
            try
            {
                await _insigniasDataAccess.DeleteInsignia(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-insignia-by-price-range")]
        public async Task<ActionResult<List<Insignia>>> GetInsigniaByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
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

                var insignias = await _insigniasDataAccess.GetInsigniaByPriceRange(minPrice, maxPrice);
                if (insignias == null || insignias.Count == 0)
                {
                    return NotFound("No insignias found in hte specified price range.");
                }
                return Ok(insignias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-insignia-by-insignia-type/{insigniaType}")]
        public async Task<ActionResult<List<Insignia>>> GetInsigniaByInsigniaType(string insigniaType)
        {
            try
            {
                var insignias = await _insigniasDataAccess.GetInsigniaByInsigniaType(insigniaType);
                if (insignias == null || insignias.Count == 0)
                {
                    return NotFound($"No insignias found of type {insigniaType}.");
                }
                return Ok(insignias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-insignia-by-origin/{origin}")]
        public async Task<ActionResult<List<Equipment>>> GetInsigniaByOrigin(string origin)
        {
            try
            {
                var insignias = await _insigniasDataAccess.GetInsigniaByOrigin(origin);
                if (insignias == null || insignias.Count == 0)
                {
                    return NotFound($"No insignia found from origin {origin}.");
                }
                return Ok(insignias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-insignia-by-era/{era}")]
        public async Task<ActionResult<List<Insignia>>> GetInsigniaByEra(string era)
        {
            try
            {
                var insignias = await _insigniasDataAccess.GetInsigniaByEra(era);
                if (insignias == null || insignias.Count == 0)
                {
                    return NotFound($"No insignias found from era {era}.");
                }
                return Ok(insignias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-insignia-by-material/{material}")]
        public async Task<ActionResult<List<Insignia>>> GetInsigniaByMaterial(string material)
        {
            try
            {
                var insignias = await _insigniasDataAccess.GetInsigniaByMaterial(material);
                if (insignias == null || insignias.Count == 0)
                {
                    return NotFound($"No insignias found made of material {material}.");
                }
                return Ok(insignias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-insignia-by-part-of-set/{partOfSet}")]
        public async Task<ActionResult<List<Insignia>>> GetInsigniaByPartOfSet(bool partOfSet = false)
        {
            try
            {
                var insignias = await _insigniasDataAccess.GetInsigniaByPartOfSet(partOfSet);
                if (insignias == null || insignias.Count == 0)
                {
                    return NotFound($"No insignias found with PartOfSet = {partOfSet}.");
                }

                return Ok(insignias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all-series-insignias/{seriesId}")]
        public async Task<ActionResult<List<Insignia>>> GetAllSeriesInsignias(int seriesId)
        {
            try
            {
                var insignias = await _insigniasDataAccess.GetAllSeriesInsignias(seriesId);
                if (insignias == null || insignias.Count == 0)
                {
                    return NotFound($"No insignias found for series ID {seriesId}.");
                }
                return Ok(insignias);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-insignia-by-storage-area/{storageAreaId}")]
        public async Task<ActionResult<List<Insignia>>> GetInsigniaByStorageArea(int storageAreaId)
        {
            try
            {
                var insignias = await _insigniasDataAccess.GetInsigniasByStorageArea(storageAreaId);
                if (insignias == null || insignias.Count == 0)
                {
                    return NotFound($"No insignias found in storage area ID {storageAreaId}.");
                }

                return Ok(insignias);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("assign-insignia-to-insignia-series/{insigniaId}/series/{seriesId}")]
        public async Task<ActionResult> UpdateAssignInsigniaToInsigniaSeries(int insigniaId, int seriesId)
        {
            try
            {
                await _insigniasDataAccess.UpdateAssignInsigniaToInsigniaSeries(insigniaId, seriesId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("assign-insignia-to-storage-area/{insigniaId}/storage-area/{storageAreaId}")]
        public async Task<ActionResult> UpdateAssignInsigniaToStorageArea(int insigniaId, int storageAreaId)
        {
            try
            {
                await _insigniasDataAccess.UpdateAssignInsigniaToStorageArea(insigniaId, storageAreaId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
