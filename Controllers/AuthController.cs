using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadDefect.Api.Data;
using RoadDefect.Api.DTOs.Auth;
using RoadDefect.Api.Helpers;
using RoadDefect.Api.Models;
using System.Security.Cryptography;
using System.Text;

namespace RoadDefect.Api.Controllers;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly JwtTokenGenerator _token;
    private readonly IMapper _mapper;

    private readonly IConfiguration _config;

    public AuthController(ApplicationDbContext db, IMapper mapper, JwtTokenGenerator token, IConfiguration config)
    {
        _db = db;
        _mapper = mapper;
        _token = token;
        _config = config;
    }

    // -------------------------------
    // REGISTER (Citizen)
    // -------------------------------
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthUserResponseDto>> Register(RegisterDto dto)
    {
        var exists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
        if (exists) return Conflict("Email already registered");

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = HashPassword(dto.Password);
        
        user.Role = UserRole.Citizen;

        if (dto.Role.HasValue)
        {
            var adminSecret = _config["AdminSecret"];
            var engineerSecret = _config["EngineerSecret"];

            if ((UserRole)dto.Role == UserRole.Admin && dto.SecretKey == adminSecret)
            {
                user.Role = UserRole.Admin;
            }
            else if ((UserRole)dto.Role == UserRole.Engineer && dto.SecretKey == engineerSecret)
            {
                user.Role = UserRole.Engineer;
            }
        }
        user.CreatedAt = DateTime.UtcNow;

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var result = _mapper.Map<AuthUserResponseDto>(user);
        return Ok(result);
    }

    // -------------------------------
    // LOGIN (returns JWT token)
    // -------------------------------
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<object>> Login(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
            return Unauthorized("Invalid credentials");

        var hashed = HashPassword(dto.Password);
        if (hashed != user.PasswordHash)
            return Unauthorized("Invalid credentials");

        var token = _token.GenerateToken(user);

        return Ok(new
        {
            token,
            user = _mapper.Map<AuthUserResponseDto>(user)
        });
    }

    // -------------------------------
    // Utility: Hash Password
    // -------------------------------
    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser(RegisterDto dto)
    {
        var user = _mapper.Map<User>(dto);
        user.PasswordHash = HashPassword(dto.Password);
        user.Role = (UserRole)(dto.Role ?? 0);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok("User created");
    }
}
