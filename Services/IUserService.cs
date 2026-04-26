using RoadDefect.Api.DTOs.Users;

namespace RoadDefect.Api.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<List<UserDto>> GetByRoleAsync(string role);
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto> CreateEngineerAsync(CreateUserDto dto);
    Task<bool> DeleteAsync(int id);
}
