using GunterBar.Application.DTOs.Bar;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.DTOs.User;

public class CreateUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateTime? BirthDate { get; set; }
    public UserRole Role { get; set; }

    // Optional bar information (only for vendors)
    public CreateBarDto? BarInfo { get; set; }
}
