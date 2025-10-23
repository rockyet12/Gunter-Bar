namespace GunterBar.Application.DTOs.User;

public class UpdateUserDto
{
    public string Name { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? DeliveryDescription { get; set; }
    public DateTime? BirthDate { get; set; }
}
