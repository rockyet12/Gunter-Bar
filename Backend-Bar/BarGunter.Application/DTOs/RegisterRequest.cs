using System.ComponentModel.DataAnnotations;
using BarGunter.Domain.Enums;

namespace BarGunter.Application.DTOs;

public class RegisterRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public Gender Gender { get; set; }

    public string Role { get; set; } = "Customer";
}
