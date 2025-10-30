using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilitaryCollectiblesBackend.CustomClasses;
using MilitaryCollectiblesBackend.DataAccessLayer;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsigniaSeriesController : ControllerBase
    {
        private readonly IInsigniaSeriesDataAccess _insigniaSeriesDataAccess;
        public InsigniaSeriesController(IInsigniaSeriesDataAccess insigniaSeriesDataAccess)
        {
            _insigniaSeriesDataAccess = insigniaSeriesDataAccess;
        }

        [HttpGet("get-insignia-series/{id}")]
        public async Task<ActionResult<InsigniaSeries>> GetInsigniaSeries(int id)
        {
            try
            {
                var insigniaSeries = await _insigniaSeriesDataAccess.GetInsigniaSeries(id);
                if (insigniaSeries == null)
                {
                    return NotFound($"InsigniaSeries with ID {id} not found.");
                }

                return Ok(insigniaSeries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all-insignia-series")]
        public async Task<ActionResult<List<InsigniaSeries>>> GetAllInsigniaSeries([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var insigniaSeries = await _insigniaSeriesDataAccess.GetAllInsigniaSeries(pageNumber, pageSize);
                if (insigniaSeries == null || insigniaSeries.Count == 0)
                {
                    return NotFound("No InsigniaSeries found.");
                }

                return Ok(insigniaSeries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("create-insignia-series")]
        public async Task<ActionResult<ResponseDto<InsigniaSeries>>> CreateInsigniaSeries([FromBody] InsigniaSeries insigniaSeries)
        {
            if (insigniaSeries.SeriesName.IsNullOrEmpty() || insigniaSeries.Description.IsNullOrEmpty())
            {
                return BadRequest("SeriesName and Description is required.");
            }

            if (insigniaSeries.SeriesName.Length > 100)
            {
                return BadRequest("SeriesName cannot exceed 100 characters.");
            }

            if (insigniaSeries.Description != null && insigniaSeries.Description.Length > 500)
            {
                return BadRequest("Description cannot exceed 500 characters.");
            }

            try
            {
                var createdInsigniaSeries = await _insigniaSeriesDataAccess.CreateInsigniaSeries(insigniaSeries);
                var response = new ResponseDto<InsigniaSeries> { CreatedObject = createdInsigniaSeries, entityType = "insigniaSeries" };
                return CreatedAtAction(nameof(GetInsigniaSeries), new { id = createdInsigniaSeries.SeriesId }, response);
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

        [HttpPut("update-insignia-series/{id}")]
        public async Task<ActionResult<ResponseDto<InsigniaSeries>>> UpdateInsigniaSeries(int id, [FromBody] InsigniaSeries insigniaSeries)
        {
            if (insigniaSeries.SeriesName.IsNullOrEmpty() || insigniaSeries.Description.IsNullOrEmpty())
            {
                return BadRequest("SeriesName and Description is required.");
            }
            if (insigniaSeries.SeriesName.Length > 100)
            {
                return BadRequest("SeriesName cannot exceed 100 characters.");
            }
            if (insigniaSeries.Description != null && insigniaSeries.Description.Length > 500)
            {
                return BadRequest("Description cannot exceed 500 characters.");
            }
            try
            {
                var updatedInsigniaSeries = await _insigniaSeriesDataAccess.UpdateInsigniaSeries(id, insigniaSeries);
                var response = new ResponseDto<InsigniaSeries> { CreatedObject = updatedInsigniaSeries, entityType = "insigniaSeries" };
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

        [HttpDelete("delete-insignia-series/{id}")]
        public async Task<IActionResult> DeleteInsigniaSeries(int id)
        {
            try
            {
                await _insigniaSeriesDataAccess.DeleteInsigniaSeries(id);
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
