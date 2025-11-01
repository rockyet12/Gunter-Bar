using GunterBar.Application.DTOs.Bar;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? DeliveryDescription { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }

    // Bar information (only for vendors)
    public BarDto? Bar { get; set; }
}
