using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilitaryCollectiblesBackend.CustomClasses;
using MilitaryCollectiblesBackend.DataAccessLayer;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtifactSeriesController : ControllerBase
    {
        private readonly IArtifactSeriesDataAccess _artifactSeriesDataAccess;
        public ArtifactSeriesController(IArtifactSeriesDataAccess artifactSeriesDataAccess)
        {
            _artifactSeriesDataAccess = artifactSeriesDataAccess;
        }

        [HttpGet("get-artifact-series/{id}")]
        public async Task<ActionResult<ArtifactSeries>> GetArtifactSeries(int id)
        {
            try
            {
                var artifactSeries = await _artifactSeriesDataAccess.GetArtifactSeries(id);
                if (artifactSeries == null)
                {
                    return NotFound($"ArtifactSeries with ID {id} not found.");
                }

                return Ok(artifactSeries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all-artifact-series")]
        public async Task<ActionResult<List<ArtifactSeries>>> GetAllArtifactSeries([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var artifactSeries = await _artifactSeriesDataAccess.GetAllArtifactSeries(pageNumber, pageSize);
                if (artifactSeries == null || artifactSeries.Count == 0)
                {
                    return NotFound("No ArtifactSeries found.");
                }

                return Ok(artifactSeries);
            } catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("create-artifact-series")]
        public async Task<ActionResult<ResponseDto<ArtifactSeries>>> CreateArtifactSeris([FromBody] ArtifactSeries artifactSeries)
        {
            if (artifactSeries.SeriesName.IsNullOrEmpty() || artifactSeries.Description.IsNullOrEmpty())
            {
                return BadRequest("SeriesName and Description is required.");
            }

            if (artifactSeries.SeriesName.Length > 100)
            {
                return BadRequest("SeriesName cannot exceed 100 characters.");
            }

            if (artifactSeries.Description != null && artifactSeries.Description.Length > 500)
            {
                return BadRequest("Description cannot exceed 500 characters.");
            }

            try
            {
                var createdArtifactSeries = await _artifactSeriesDataAccess.CreateArtifactSeries(artifactSeries);
                var response = new ResponseDto<ArtifactSeries> { CreatedObject = createdArtifactSeries, entityType = "artifactSeries" };
                return CreatedAtAction(nameof(GetArtifactSeries), new { id = createdArtifactSeries.SeriesId }, response);
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

        [HttpPut("update-artifact-series/{id}")]
        public async Task<ActionResult<ResponseDto<ArtifactSeries>>> UpdateArtifactSeries(int id, [FromBody] ArtifactSeries artifactSeries)
        {
            if (artifactSeries.SeriesName.IsNullOrEmpty() || artifactSeries.Description.IsNullOrEmpty())
            {
                return BadRequest("SeriesName and Description is required.");
            }

            if (artifactSeries.SeriesName.Length > 100)
            {
                return BadRequest("SeriesName cannot exceed 100 characters.");
            }

            if (artifactSeries.Description != null && artifactSeries.Description.Length > 500)
            {
                return BadRequest("Description cannot exceed 500 characters.");
            }

            try
            {
                var updatedArtifactSeries = await _artifactSeriesDataAccess.UpdateArtifactSeries(id, artifactSeries);
                var response = new ResponseDto<ArtifactSeries> { CreatedObject = updatedArtifactSeries, entityType = "artifactSeries" };
                return Ok(response);
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete-artifact-series/{id}")]
        public async Task<ActionResult> DeleteArtifactSeries(int id)
        {
            try
            {
                await _artifactSeriesDataAccess.DeleteArtifactSeries(id);
                return NoContent();
            }
            catch(InvalidOperationException ex)
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
