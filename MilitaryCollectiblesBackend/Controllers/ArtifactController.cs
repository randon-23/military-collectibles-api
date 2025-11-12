using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilitaryCollectiblesBackend.CustomClasses;
using MilitaryCollectiblesBackend.DataAccessLayer;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtifactController : ControllerBase
    {
        private readonly IArtifactsDataAccess _artifactsDataAccess;

        public ArtifactController(IArtifactsDataAccess artifactsDataAccess)
        {
            _artifactsDataAccess = artifactsDataAccess;
        }

        [HttpGet("get-artifact/{id}")]
        public async Task<ActionResult<Artifact>> GetArtifact(int id)
        {
            try
            {
                var artifact = await _artifactsDataAccess.GetArtifact(id);
                if (artifact == null)
                {
                    return NotFound($"Artifact with ID {id} not found.");
                }

                return Ok(artifact);
            } catch(Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-artifacts")]
        public async Task<ActionResult<List<Artifact>>> GetAllArtifacts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            try
            {
                var artifacts = await _artifactsDataAccess.GetAllArtifacts(pageNumber, pageSize);
                if(artifacts == null || artifacts.Count == 0)
                {
                    return NotFound("No artifacts found.");
                }

                return Ok(artifacts);
            } catch(Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpPost("create-artifact")]
        public async Task<ActionResult<ResponseDto<Artifact>>> CreateArtifact([FromBody] Artifact artifact)
        {
            if (artifact.Name.IsNullOrEmpty() || artifact.Description.IsNullOrEmpty() || artifact.ArtifactType.ArtifactTypeName.IsNullOrEmpty())
            {
                return BadRequest("Name, Description, and ArtifactType are required fields.");
            }

            if (!artifact.Availability)
            {
                return BadRequest("Artifacts must be available at point of creation.");
            }

            if (artifact.Price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }

            try
            {
                var createdArtifact = await _artifactsDataAccess.CreateArtifact(artifact);
                var response = new ResponseDto<Artifact> { CreatedObject = createdArtifact, entityType = "artifact" };
                return CreatedAtAction(nameof(GetArtifact), new { id = createdArtifact.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpPut("update-artifact/{id}")]
        public async Task<ActionResult<Artifact>> UpdateArtifact(int id, [FromBody] Artifact artifact)
        {
            if (artifact.Name.IsNullOrEmpty() || artifact.Description.IsNullOrEmpty() || artifact.ArtifactType.ArtifactTypeName.IsNullOrEmpty())
            {
                return BadRequest("Name, Description, and ArtifactType are required fields.");
            }
            if (artifact.Price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }

            try
            {
                var updatedArtifact = await _artifactsDataAccess.UpdateArtifact(id, artifact);
                var response = new ResponseDto<Artifact> { CreatedObject = updatedArtifact, entityType = "artifact" };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete-artifact/{id}")]
        public async Task<ActionResult> DeleteArtifact(int id)
        {
            try
            {
                await _artifactsDataAccess.DeleteArtifact(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-artifact-by-price-range")]
        public async Task<ActionResult<List<Artifact>>> GetArtifactByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            try
            {
                if(minPrice < 0 || maxPrice < 0)
                {
                    return BadRequest("Price values cannot be negative.");
                }
                if(minPrice > maxPrice)
                {
                    return BadRequest("Minimum price cannot be greater than maximum price.");
                }

                var artifacts = await _artifactsDataAccess.GetArtifactByPriceRange(minPrice, maxPrice);
                if(artifacts == null || artifacts.Count == 0)
                {
                    return NotFound("No artifacts found in the specified price range.");
                }
                return Ok(artifacts);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-artifact-by-artifact-type/{artifactType}")]
        public async Task<ActionResult<List<Artifact>>> GetArtifactByArtifactType(string artifactType)
        {
            try
            {
                var artifacts = await _artifactsDataAccess.GetArtifactByArtifactType(artifactType);
                if (artifacts == null || artifacts.Count == 0)
                {
                    return NotFound($"No artifacts found of type {artifactType}.");
                }

                return Ok(artifacts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-artifact-by-origin/{origin}")]
        public async Task<ActionResult<List<Artifact>>> GetArtifactByOrigin(string origin)
        {
            try
            {
                var artifacts = await _artifactsDataAccess.GetArtifactByOrigin(origin);
                if (artifacts == null || artifacts.Count == 0)
                {
                    return NotFound($"No artifacts found from origin {origin}.");
                }
                return Ok(artifacts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-artifact-by-era/{era}")]
        public async Task<ActionResult<List<Artifact>>> GetArtifactByEra(string era)
        {
            try
            {
                var artifacts = await _artifactsDataAccess.GetArtifactByEra(era);
                if (artifacts == null || artifacts.Count == 0)
                {
                    return NotFound($"No artifacts found from era {era}.");
                }
                return Ok(artifacts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all-series-artifacts/{seriesId}")]
        public async Task<ActionResult<List<Artifact>>> GetAllSeriesArtifacts(int seriesId)
        {
            try
            {
                var artifacts = await _artifactsDataAccess.GetAllSeriesArtifacts(seriesId);
                if (artifacts == null || artifacts.Count == 0)
                {
                    return NotFound($"No artifacts found for series ID {seriesId}.");
                }

                return Ok(artifacts);
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

        [HttpGet("get-artifacts-by-storage-area/{storageAreaId}")]
        public async Task<ActionResult<List<Artifact>>> GetArtifactsByStorageArea(int storageAreaId)
        {
            try
            {
                var artifacts = await _artifactsDataAccess.GetArtifactsByStorageArea(storageAreaId);
                if (artifacts == null || artifacts.Count == 0)
                {
                    return NotFound($"No artifacts found in storage area ID {storageAreaId}.");
                }

                return Ok(artifacts);
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

        [HttpPut("assign-artifact-to-series/{artifactId}/series/{seriesId}")]
        public async Task<ActionResult> UpdateAssignArtifactToArtifactSeries(int artifactId, int seriesId)
        {
            try
            {
                await _artifactsDataAccess.UpdateAssignArtifactToArtifactSeries(artifactId, seriesId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpPut("assign-artifact-to-storage-area/{artifactId}/storage-area/{storageAreaId}")]
        public async Task<ActionResult> UpdateAssignArtifactToStorageArea(int artifactId, int storageAreaId)
        {
            try
            {
                await _artifactsDataAccess.UpdateAssignArtifactToStorageArea(artifactId, storageAreaId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
