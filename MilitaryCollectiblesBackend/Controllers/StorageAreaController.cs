using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilitaryCollectiblesBackend.CustomClasses;
using MilitaryCollectiblesBackend.DataAccessLayer;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageAreaController : ControllerBase
    {
        private readonly IStorageAreaDataAccess _storageAreaDataAccess;
        public StorageAreaController(IStorageAreaDataAccess storageAreaDataAccess)
        {
            _storageAreaDataAccess = storageAreaDataAccess;
        }

        [HttpGet("get-storage-area/{id}")]
        public async Task<ActionResult<StorageArea>> GetStorageArea(int id)
        {
            try
            {
                var storageArea = await _storageAreaDataAccess.GetStorageArea(id);
                if (storageArea == null)
                {
                    return NotFound($"StorageArea with ID {id} not found.");
                }
                return Ok(storageArea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all-storage-areas")]
        public async Task<ActionResult<List<StorageArea>>> GetAllStorageAreas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var storageAreas = await _storageAreaDataAccess.GetAllStorageAreas(pageNumber, pageSize);
                if (storageAreas == null || storageAreas.Count == 0)
                {
                    return NotFound("No StorageAreas found.");
                }
                return Ok(storageAreas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("create-storage-area")]
        public async Task<ActionResult<ResponseDto<StorageArea>>> CreateStorageArea([FromBody] StorageArea storageArea)
        {
            if (storageArea.StorageAreaName.IsNullOrEmpty())
            {
                return BadRequest("StorageAreaName is required.");
            }

            if (storageArea.StorageAreaNotes != null && storageArea.StorageAreaNotes.Length > 100)
            {
                return BadRequest("StorageAreaNotes cannot exceed 100 characters.");
            }

            try
            {
                var createdStorageArea = await _storageAreaDataAccess.CreateStorageArea(storageArea);
                var response = new ResponseDto<StorageArea> { CreatedObject = createdStorageArea, entityType = "storageArea" };
                return CreatedAtAction(nameof(GetStorageArea), new { id = createdStorageArea.Id }, response);
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

        [HttpPut("update-storage-area/{id}")]
        public async Task<ActionResult<ResponseDto<StorageArea>>> UpdateStorageArea(int id, [FromBody] StorageArea storageArea)
        {
            if (storageArea.StorageAreaName.IsNullOrEmpty())
            {
                return BadRequest("StorageAreaName is required.");
            }

            if (storageArea.StorageAreaNotes != null && storageArea.StorageAreaNotes.Length > 100)
            {
                return BadRequest("StorageAreaNotes cannot exceed 100 characters.");
            }

            try
            {
                var updatedStorageArea = await _storageAreaDataAccess.UpdateStorageArea(id, storageArea);
                var response = new ResponseDto<StorageArea> { CreatedObject = updatedStorageArea, entityType = "storageArea" };
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

        [HttpDelete("delete-storage-area/{id}")]
        public async Task<ActionResult> DeleteStorageARea(int id)
        {
            try
            {
                await _storageAreaDataAccess.DeleteStorageArea(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
