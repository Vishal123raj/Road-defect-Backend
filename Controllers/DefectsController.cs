using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoadDefect.Api.DTOs.Defects;
using RoadDefect.Api.Models;
using RoadDefect.Api.Services;
using Microsoft.AspNetCore.Http;

namespace RoadDefect.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DefectsController : ControllerBase
{
    private readonly IDefectService _service;

    private readonly IDefectImageService _imageService;

    public DefectsController(IDefectService service, IDefectImageService imageService)
    {
        _service = service;
        _imageService = imageService;
    }


    // GET: api/v1/defects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DefectListDto>>> GetDefects(
        [FromQuery] DefectStatus? status,
        [FromQuery] int? areaId,
        [FromQuery] int? assignedEngineerId)
    {
        var result = await _service.GetAllAsync(status, areaId, assignedEngineerId);
        return Ok(result);
    }

    // GET: api/v1/defects/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DefectDetailsDto>> GetDefect(int id)
    {
        var defect = await _service.GetByIdAsync(id);
        if (defect == null)
            return NotFound();

        return Ok(defect);
    }

    // POST: api/v1/defects
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<DefectDetailsDto>> CreateDefect([FromBody] DefectCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetDefect), new { id = created.Id }, created);
    }

    // PUT: api/v1/defects/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDefect(int id, [FromBody] DefectUpdateDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        if (!success)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/v1/defects/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDefect(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }

    //// POST: api/v1/defects/{id}/images
    //[HttpPost("{id:int}/images")]
    //public async Task<ActionResult<DefectDetailsDto>> UploadImage(
    //    int id,
    //    IFormFile file,
    //    [FromQuery] bool isBeforeRepair = true)
    //{
    //    if (file == null || file.Length == 0)
    //        return BadRequest("Image file is required.");

    //    var result = await _imageService.AddImageAsync(id, file, isBeforeRepair);
    //    if (result == null)
    //        return NotFound("Defect not found.");

    //    // returns updated defect with Images list populated
    //    return Ok(result);
    //}

}
