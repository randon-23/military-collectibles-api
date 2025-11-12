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
    public class MechanicalEquipmentController : ControllerBase
    {
        private readonly IMechanicalEquipmentDataAccess _mechanicalEquipmentDataAccess;
        public MechanicalEquipmentController(IMechanicalEquipmentDataAccess mechanicalEquipmentDataAccess)
        {
            _mechanicalEquipmentDataAccess = mechanicalEquipmentDataAccess;
        }

        [HttpGet("get-mech-equipment/{id}")]
        public async Task<ActionResult<MechanicalEquipment>> GetMechanicalEquipment(int id)
        {
            try
            {
                var mechEquipment = await _mechanicalEquipmentDataAccess.GetMechanicalEquipment(id);
                if (mechEquipment == null)
                {
                    return NotFound($"Mechanical equipment with ID {id} not found.");
                }
                return Ok(mechEquipment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all-mech-equipments")]
        public async Task<ActionResult<List<MechanicalEquipment>>> GetAllMechanicalEquipments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            try
            {
                var mechEquipments = await _mechanicalEquipmentDataAccess.GetAllMechanicalEquipments(pageNumber, pageSize);
                if (mechEquipments == null || mechEquipments.Count == 0)
                {
                    return NotFound("No mechanical equipments found.");
                }
                return Ok(mechEquipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("create-mech-equipment")]
        public async Task<ActionResult<ResponseDto<MechanicalEquipment>>> CreateMechanicalEquipment([FromBody] MechanicalEquipment mechEquipment)
        {
            if (mechEquipment.Name.IsNullOrEmpty() || mechEquipment.Description.IsNullOrEmpty() || mechEquipment.MechanicalEquipmentType.MechanicalEquipmentTypeName.IsNullOrEmpty())
            {
                return BadRequest("Name, Description, and MechanicalEquipmentType are required fields.");
            }

            if (!mechEquipment.Availability)
            {
                return BadRequest("New mechanical equipment must be marked as available.");
            }

            if (mechEquipment.Price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }

            // Any validations for CaliberSpec, VehicleModel, SerialNumber and Manufacturer can be added here as needed (since there are no database-level checks)

            try
            {
                var createdMechEquipment = await _mechanicalEquipmentDataAccess.CreateMechanicalEquipment(mechEquipment);
                var response = new ResponseDto<MechanicalEquipment> { CreatedObject = createdMechEquipment, entityType = "mechanicalEquipment" };
                return CreatedAtAction(nameof(GetMechanicalEquipment), new { id = createdMechEquipment.Id }, response);
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

        [HttpPut("update-mech-equipment/{id}")]
        public async Task<ActionResult<ResponseDto<MechanicalEquipment>>> UpdateMechanicalEquipment(int id, [FromBody] MechanicalEquipment mechEquipment)
        {
            if (mechEquipment.Name.IsNullOrEmpty() || mechEquipment.Description.IsNullOrEmpty() || mechEquipment.MechanicalEquipmentType.MechanicalEquipmentTypeName.IsNullOrEmpty())
            {
                return BadRequest("Name, Description, and MechanicalEquipmentType are required fields.");
            }
            if (mechEquipment.Price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }

            try
            {
                var updatedMechEquipment = await _mechanicalEquipmentDataAccess.UpdateMechanicalEquipment(id, mechEquipment);
                var response = new ResponseDto<MechanicalEquipment> { CreatedObject = updatedMechEquipment, entityType = "mechanicalEquipment" };

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

        [HttpDelete("delete-mech-equipment/{id}")]
        public async Task<ActionResult> DeleteMechanicalEquipment(int id)
        {
            try
            {
                await _mechanicalEquipmentDataAccess.DeleteMechanicalEquipment(id);
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

        [HttpGet("get-mechequipment-by-price-range")]
        public async Task<ActionResult<List<MechanicalEquipment>>> GetMechanicalEquipmentByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            try
            {
                if (minPrice < 0 || maxPrice < 0)
                {
                    return BadRequest("Price values cannot be negative.");
                }
                if (minPrice > maxPrice)
                {
                    return BadRequest("Minimum price cannot be greater than maximum price.");
                }
                var mechEquipments = await _mechanicalEquipmentDataAccess.GetMechanicalEquipmentByPriceRange(minPrice, maxPrice);
                if (mechEquipments == null || mechEquipments.Count == 0)
                {
                    return NotFound("No mechanical equipments found in the specified price range.");
                }
                return Ok(mechEquipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-mechequipment-by-type")]
        public async Task<ActionResult<List<MechanicalEquipment>>> GetMechanicalEquipmentByType([FromQuery] string mechanicalEquipmentType)
        {
            try
            {
                var mechEquipments = await _mechanicalEquipmentDataAccess.GetMechanicalEquipmentByType(mechanicalEquipmentType);
                if (mechEquipments == null || mechEquipments.Count == 0)
                {
                    return NotFound($"No mechanical equipments found of type '{mechanicalEquipmentType}'.");
                }
                return Ok(mechEquipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-mechequipment-by-caliber")]
        public async Task<ActionResult<List<MechanicalEquipment>>> GetMechanicalEquipmentByCaliberSpec([FromQuery] string caliberSpec)
        {
            if (caliberSpec.IsNullOrEmpty())
            {
                return BadRequest("Caliber specification cannot be empty.");
            }

            if (!Regex.IsMatch(caliberSpec, @"^[\w\s\.\-\/]+$"))
            {
                return BadRequest("Caliber specification contains invalid characters.");
            }

            if (caliberSpec.Length > 50)
            {
                return BadRequest("Caliber specification exceeds maximum length of 50 characters.");
            }

            try
            {
                var mechEquipments = await _mechanicalEquipmentDataAccess.GetMechanicalEquipmentByCaliberSpec(caliberSpec);
                if (mechEquipments == null || mechEquipments.Count == 0)
                {
                    return NotFound($"No mechanical equipments found with caliber specification '{caliberSpec}'.");
                }
                return Ok(mechEquipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-mechequipment-by-vehicle-model")]
        public async Task<ActionResult<List<MechanicalEquipment>>> GetMechanicalEquipmentByVehicleModel([FromQuery] string vehicleModel)
        {
            try
            {
                var mechEquipments = await _mechanicalEquipmentDataAccess.GetMechanicalEquipmentByVehicleModel(vehicleModel);
                if (mechEquipments == null || mechEquipments.Count == 0)
                {
                    return NotFound($"No mechanical equipments found for vehicle model '{vehicleModel}'.");
                }
                return Ok(mechEquipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-mechequipment-by-manufacturer")]
        public async Task<ActionResult<List<MechanicalEquipment>>> GetMechanicalEquipmentByManufacturer([FromQuery] string manufacturer)
        {
            try
            {
                var mechEquipments = await _mechanicalEquipmentDataAccess.GetMechanicalEquipmentByManufacturer(manufacturer);
                if (mechEquipments == null || mechEquipments.Count == 0)
                {
                    return NotFound($"No mechanical equipments found from manufacturer '{manufacturer}'.");
                }
                return Ok(mechEquipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-mechequipment-by-origin/{origin}")]
        public async Task<ActionResult<List<MechanicalEquipment>>> GetMechanicalEquipmentByOrigin(string origin)
        {
            try
            {
                var mechEquipments = await _mechanicalEquipmentDataAccess.GetMechanicalEquipmentByOrigin(origin);
                if (mechEquipments == null || mechEquipments.Count == 0)
                {
                    return NotFound($"No mechanical equipments found from origin {origin}.");
                }
                return Ok(mechEquipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-mechequipment-by-era/{era}")]
        public async Task<ActionResult<List<MechanicalEquipment>>> GetMechanicalEquipmentByEra(string era)
        {
            try
            {
                var equipments = await _mechanicalEquipmentDataAccess.GetMechanicalEquipmentByEra(era);
                if (equipments == null || equipments.Count == 0)
                {
                    return NotFound($"No mechanical equipments found from era {era}.");
                }
                return Ok(equipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-mechequipment-by-material/{material}")]
        public async Task<ActionResult<List<MechanicalEquipment>>> GetMechanicalEquipmentByMaterial(string material)
        {
            try
            {
                var mechEquipments = await _mechanicalEquipmentDataAccess.GetMechanicalEquipmentByMaterial(material);
                if (mechEquipments == null || mechEquipments.Count == 0)
                {
                    return NotFound($"No mechanical equipments found made of material {material}.");
                }
                return Ok(mechEquipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-mechequipments-by-storage-area/{storageAreaId}")]
        public async Task<ActionResult<List<MechanicalEquipment>>> GetMechanicalEquipmentByStorageArea(int storageAreaId)
        {
            try
            {
                var mechEquipments = await _mechanicalEquipmentDataAccess.GetMechanicalEquipmentsByStorageArea(storageAreaId);
                if (mechEquipments == null || mechEquipments.Count == 0)
                {
                    return NotFound($"No mechanical equipments found in storage area ID {storageAreaId}.");
                }
                return Ok(mechEquipments);
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

        [HttpPut("assign-mechequipment-to-storage-area/{equipmentId}/storage-area/{storageAreaId}")]
        public async Task<ActionResult> UpdateAssignMechanicalEquipmentToStorageArea(int equipmentId, int storageAreaId)
        {
            try
            {
                await _mechanicalEquipmentDataAccess.UpdateAssignMechanicalEquipmentToStorageArea(equipmentId, storageAreaId);
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
