using Microsoft.AspNetCore.Mvc;
using RoadDefect.Api.Services;

namespace RoadDefect.Api.Controllers;

[ApiController]
[Route("api/v1/Defects")]
public class DefectImagesController : ControllerBase
{
    private readonly IDefectImageService _service;

    public DefectImagesController(IDefectImageService service)
    {
        _service = service;
    }

    [HttpPost("{id:int}/images")]
    public async Task<IActionResult> UploadImage(
        int id,
        [FromQuery] bool isBeforeRepair,
        IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var imageUrl = await _service.AddImageAsync(id, file, isBeforeRepair);

        if (imageUrl == null)
            return NotFound("Defect not found.");

        return Ok(new { defectId = id, imageUrl });
    }
}
