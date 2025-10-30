using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilitaryCollectiblesBackend.CustomClasses;
using MilitaryCollectiblesBackend.DataAccessLayer;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiteratureSeriesController : ControllerBase
    {
        private readonly ILiteratureSeriesDataAccess _literatureSeriesDataAccess;

        public LiteratureSeriesController(ILiteratureSeriesDataAccess literatureSeriesDataAccess)
        {
            _literatureSeriesDataAccess = literatureSeriesDataAccess;
        }

        [HttpGet("get-literature-series/{id}")]
        public async Task<ActionResult<LiteratureSeries>> GetLiteratureSeries(int id)
        {
            try
            {
                var literatureSeries = await _literatureSeriesDataAccess.GetLiteratureSeries(id);
                if(literatureSeries == null)
                {
                    return NotFound($"LiteratureSeries with ID {id} not found.");
                }
                return Ok(literatureSeries);
            } catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all-literature-series")]
        public async Task<ActionResult<List<LiteratureSeries>>> GetAllLiteratureSeries([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var literatureSeries = await _literatureSeriesDataAccess.GetAllLiteratureSeries(pageNumber, pageSize);
                if(literatureSeries == null || literatureSeries.Count == 0)
                {
                    return NotFound("No LiteratureSeries found.");
                }

                return Ok(literatureSeries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("create-literature-series")]
        public async Task<ActionResult<ResponseDto<LiteratureSeries>>> CreateLiteratureSeries([FromBody] LiteratureSeries literatureSeries)
        {
            if (literatureSeries.SeriesName.IsNullOrEmpty() || literatureSeries.Description.IsNullOrEmpty())
            {
                return BadRequest("SeriesName and Description is required.");
            }

            if(literatureSeries.SeriesName.Length > 100)
            {
                return BadRequest("SeriesName cannot exceed 100 characters.");
            }

            if(literatureSeries.Description != null && literatureSeries.Description.Length > 500)
            {
                return BadRequest("Description cannot exceed 500 characters.");
            }

            try
            {
                var createdLiteratureSeries = await _literatureSeriesDataAccess.CreateLiteratureSeries(literatureSeries);
                var response = new ResponseDto<LiteratureSeries> { CreatedObject = createdLiteratureSeries, entityType = "literatureSeries" };
                return CreatedAtAction(nameof(GetLiteratureSeries), new {id = createdLiteratureSeries.SeriesId }, response);
            } 
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update-literature-series/{id}")]
        public async Task<ActionResult<ResponseDto<LiteratureSeries>>> UpdateLiteratureSeries(int id, [FromBody] LiteratureSeries literatureSeries)
        {
            if(literatureSeries.SeriesName.IsNullOrEmpty() || literatureSeries.Description.IsNullOrEmpty())
            {
                return BadRequest("SeriesName and Description is required.");
            }

            if (literatureSeries.SeriesName.Length > 100)
            {
                return BadRequest("SeriesName cannot exceed 100 characters.");
            }

            if (literatureSeries.Description != null && literatureSeries.Description.Length > 500)
            {
                return BadRequest("Description cannot exceed 500 characters.");
            }

            try
            {
                var updatedLiteratureSeries = await _literatureSeriesDataAccess.UpdateLiteratureSeries(id, literatureSeries);
                var response = new ResponseDto<LiteratureSeries> { CreatedObject = updatedLiteratureSeries, entityType = "literatureSeries" };
                return Ok(response);
            } 
            catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete-literature-series/{id}")]
        public async Task<ActionResult> DeleteLiteratureSeries(int id)
        {
            try
            {
                await _literatureSeriesDataAccess.DeleteLiteratureSeries(id);
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
