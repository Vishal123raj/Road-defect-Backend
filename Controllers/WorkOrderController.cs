using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoadDefect.Api.DTOs.WorkOrders;
using RoadDefect.Api.Services;

namespace RoadDefect.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WorkOrderController : ControllerBase
{
    private readonly IWorkOrderService _service;

    public WorkOrderController(IWorkOrderService service)
    {
        _service = service;
    }

    // 🔹 ADMIN - Get all work orders
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkOrderDetailsDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    // 🔹 Get by ID
    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkOrderDetailsDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    // 🔥 ENGINEER - Get my work orders
    [Authorize(Roles = "Engineer")]
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<WorkOrderDetailsDto>>> GetMyOrders()
    {
        //var userId = int.Parse(User.FindFirst("id")!.Value);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var result = await _service.GetByEngineerAsync(userId);
        return Ok(result);
    }

    // 🔹 ADMIN - Create WorkOrder
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<WorkOrderDetailsDto>> Create(WorkOrderCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // 🔹 ENGINEER - Update Status
    [Authorize(Roles = "Engineer")]
    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] WorkOrderUpdateStatusDto dto)
    {
        var success = await _service.UpdateStatusAsync(id, dto);
        if (!success) return NotFound();

        return NoContent();
    }

    // 🔥 ADD COMMENT (progress update)
    [Authorize(Roles = "Engineer")]
    [HttpPost("{id:int}/update")]
    public async Task<IActionResult> AddUpdate(int id,   [FromBody] WorkOrderUpdateDto dto)// ✅ IMPORTANT
    {
        var success = await _service.AddUpdateAsync(id, dto);

        if (!success) return NotFound();

        return Ok();
    }

    // 🔥 GET COMMENTS
    [HttpGet("{id:int}/updates")]
    public async Task<ActionResult> GetUpdates(int id)
    {
        return Ok(await _service.GetUpdatesAsync(id));
    }
}