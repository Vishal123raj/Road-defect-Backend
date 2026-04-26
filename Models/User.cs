namespace RoadDefect.Api.Models;

public enum UserRole
{
    Citizen = 0,
    Engineer = 1,
    Admin = 2
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Citizen;
    public int? AreaId { get; set; }
    public Area? Area { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
