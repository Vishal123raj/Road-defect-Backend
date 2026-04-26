using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadDefect.Api.Data;
using RoadDefect.Api.DTOs.Users;
using RoadDefect.Api.Models;
using System.Security.Cryptography;
using System.Text;

namespace RoadDefect.Api.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UserService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _db.Users.ToListAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<List<UserDto>> GetByRoleAsync(string role)
    {
        var match = Enum.TryParse<UserRole>(role, true, out var parsedRole);
        if (!match) return new List<UserDto>();

        var users = await _db.Users
            .Where(u => u.Role == parsedRole)
            .ToListAsync();

        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateEngineerAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            AreaId = dto.AreaId,
            PasswordHash = HashPassword(dto.Password),
            Role = UserRole.Engineer,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}
