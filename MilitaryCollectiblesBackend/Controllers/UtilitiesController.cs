using Microsoft.AspNetCore.Mvc;
using MilitaryCollectiblesBackend.DataAccessLayer.Services;

namespace MilitaryCollectiblesBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilitiesController : ControllerBase
    // Single Responsibility Principle: Controllers should have only one reason to change; adding search logic to a "utilities" controller violates this and introduces complexity
    {
        private readonly IPhotoUpdater _photoUpdater;
        public UtilitiesController(IPhotoUpdater photoUpdater) 
        {
            _photoUpdater = photoUpdater;
        }

        [HttpPost("upload")] //To be used for uploading images when creating and updating entities as part of a multipart/form-data request sequential call
        // Returns 200 with message if no file attached or if file upload fails, otherwise returns 200 with success message and file path
        public async Task<ActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string entityType, [FromForm] int entityId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return StatusCode(200, new
                    {
                        message = "Entity created successfully with no photo attached."
                    });
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var allowedEntityTypes = new[] { "literature", "artifact", "insignia", "equipment", "mechanicalequipment" };
                
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return StatusCode(200, new
                    {
                        message = "Entity created successfully, but file upload failed due to invalid file type. Only JPG and PNG allowed."
                    });
                }

                if (!allowedEntityTypes.Contains(entityType.ToLower()))
                {
                    return StatusCode(200, new
                    {
                        message = $"Entity created successfully, but file upload failed due to unsupported entity type: {entityType}."
                    });
                }

                // File saving and updating the photo URL in DB
                string basePath = @"\HOME-BASE\homebase-fs";
                string folderPath = Path.Combine(basePath, entityType.ToLower() + "images", entityId.ToString());
                string fileName = $"{entityId}_{Path.GetFileName(file.FileName)}";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                await _photoUpdater.UpdatePhotoUrlAsync(entityType, entityId, fullPath);

                return Ok(new { message = "File uploaded successfully", path = fullPath });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(200, new
                {
                    message = "Entity created successfully, but file upload failed due to invalid operation.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new
                {
                    message = "Entity created successfully, but file upload failed.",
                    error = ex.Message
                });
            }
        }
    }
}
