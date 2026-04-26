using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoadDefect.Api.DTOs.Users;
using RoadDefect.Api.Services;

namespace RoadDefect.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("role/{role}")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetByRole(string role)
    {
        return Ok(await _service.GetByRoleAsync(role));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("engineer")]
    public async Task<ActionResult<UserDto>> CreateEngineer(CreateUserDto dto)
    {
        var created = await _service.CreateEngineerAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpGet("engineers")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetEngineers()
    {
        return Ok(await _service.GetByRoleAsync("Engineer"));
    }


}
