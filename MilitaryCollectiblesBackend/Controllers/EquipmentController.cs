using Microsoft.AspNetCore.Mvc;
using MilitaryCollectiblesBackend.DataAccessLayer;
using Microsoft.IdentityModel.Tokens;
using MilitaryCollectiblesBackend.CustomClasses;
using MilitaryCollectiblesBackend.DataAccessLayer;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentDataAccess _equipmentDataAccess;
        public EquipmentController(IEquipmentDataAccess equipmentDataAccess)
        {
            _equipmentDataAccess = equipmentDataAccess;
        }

        [HttpGet("get-equipment/{id}")]
        public async Task<ActionResult<Equipment>> GetEquipment(int id)
        {
            try
            {
                var equipment = await _equipmentDataAccess.GetEquipment(id);
                if(equipment == null)
                {
                    return NotFound($"Equipment with ID {id} not found.");
                }
                return Ok(equipment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-equipments")]
        public async Task<ActionResult<List<Equipment>>> GetAllEquipments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var equipments = await _equipmentDataAccess.GetAllEquipments(pageNumber, pageSize);
                if (equipments == null || equipments.Count == 0)
                {
                    return NotFound("No equipments found.");
                }
                return Ok(equipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("create-equipment")]
        public async Task<ActionResult<List<ResponseDto<Equipment>>>> CreateEquipment([FromBody] Equipment equipment)
        {
            if(equipment.Name.IsNullOrEmpty() || equipment.Description.IsNullOrEmpty() || equipment.EquipmentType.EquipmentTypeName.IsNullOrEmpty())
            {
                return BadRequest("Name, Description, and EquipmentType are required fields.");
            }

            if(!equipment.Availability)
            {
                return BadRequest("New equipment must be available.");
            }

            if(equipment.Price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }

            try
            {
                var createdEquipment = await _equipmentDataAccess.CreateEquipment(equipment);
                var response = new ResponseDto<Equipment> { CreatedObject = createdEquipment, entityType = "equipment" };
                return CreatedAtAction(nameof(GetEquipment), new { id = createdEquipment.Id }, response);
            }    
            catch(InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update-equipment/{id}")]
        public async Task<ActionResult<ResponseDto<Equipment>>> UpdateEquipment(int id, [FromBody] Equipment equipment)
        {
            if(equipment.Name.IsNullOrEmpty() || equipment.Description.IsNullOrEmpty() || equipment.EquipmentType.EquipmentTypeName.IsNullOrEmpty())
            {
                return BadRequest("Name, Description, and EquipmentType are required fields.");
            }
            if(equipment.Price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }

            try
            {
                var updatedEquipment = await _equipmentDataAccess.UpdateEquipment(id, equipment);
                var response = new ResponseDto<Equipment> { CreatedObject = updatedEquipment, entityType = "equipment" };

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

        [HttpDelete("delete-equipment/{id}")]
        public async Task<ActionResult> DeleteEquipment(int id)
        {
            try
            {
                await _equipmentDataAccess.DeleteEquipment(id);
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

        [HttpGet("get-equipments-by-price-range")]
        public async Task<ActionResult<List<Equipment>>> GetEquipmentByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            if(minPrice < 0 || maxPrice < 0)
            {
                return BadRequest("Prices cannot be negative.");
            }
            if(minPrice > maxPrice)
            {
                return BadRequest("minPrice cannot be greater than maxPrice.");
            }
            try
            {
                var equipments = await _equipmentDataAccess.GetEquipmentByPriceRange(minPrice, maxPrice);
                if (equipments == null || equipments.Count == 0)
                {
                    return NotFound("No equipments found in the specified price range.");
                }
                return Ok(equipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-equipments-by-equipment-type/{equipmentType}")]
        public async Task<ActionResult<List<Equipment>>> GetEquipmentByEquipmentType(string equipmentType)
        {
            try
            {
                var equipments = await _equipmentDataAccess.GetEquipmentByEquipmentType(equipmentType);
                if (equipments == null || equipments.Count == 0)
                {
                    return NotFound($"No equipments found of type {equipmentType}.");
                }

                return Ok(equipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-equipments-by-origin/{origin}")]
        public async Task<ActionResult<List<Equipment>>> GetEquipmentByOrigin(string origin)
        {
            try
            {
                var equipments = await _equipmentDataAccess.GetEquipmentByOrigin(origin);
                if (equipments == null || equipments.Count == 0)
                {
                    return NotFound($"No equipments found from origin {origin}.");
                }
                return Ok(equipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-equipments-by-era/{era}")]
        public async Task<ActionResult<List<Equipment>>> GetEquipmentByEra(string era)
        {
            try
            {
                var equipments = await _equipmentDataAccess.GetEquipmentByEra(era);
                if (equipments == null || equipments.Count == 0)
                {
                    return NotFound($"No equipments found from era {era}.");
                }
                return Ok(equipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-equipments-by-material/{material}")]
        public async Task<ActionResult<List<Equipment>>> GetEquipmentByMaterial(string material)
        {
            try
            {
                var equipments = await _equipmentDataAccess.GetEquipmentByMaterial(material);
                if (equipments == null || equipments.Count == 0)
                {
                    return NotFound($"No equipments found made of material {material}.");
                }
                return Ok(equipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-equipments-by-storage-area/{storageAreaId}")]
        public async Task<ActionResult<List<Equipment>>> GetEquipmentsByStorageArea(int storageAreaId)
        {
            try
            {
                var equipments = await _equipmentDataAccess.GetEquipmentsByStorageArea(storageAreaId);
                if (equipments == null || equipments.Count == 0)
                {
                    return NotFound($"No equipments found in storage area ID {storageAreaId}.");
                }
                return Ok(equipments);
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

        [HttpPut("assign-equipment-to-storage-area/{equipmentId}/storage-area/{storageAreaId}")]
        public async Task<ActionResult> UpdateAssignEquipmentToStorageArea(int equipmentId, int storageAreaId)
        {
            try
            {
                await _equipmentDataAccess.UpdateAssignEquipmentToStorageArea(equipmentId, storageAreaId);
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
