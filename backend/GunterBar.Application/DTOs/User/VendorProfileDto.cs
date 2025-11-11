namespace GunterBar.Application.DTOs.User;

public class VendorProfileDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ProfileImageUrl { get; set; }
    public BarDto? Bar { get; set; }
}

public class BarDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? ImageUrl { get; set; }
    public string? OpeningHours { get; set; }
}