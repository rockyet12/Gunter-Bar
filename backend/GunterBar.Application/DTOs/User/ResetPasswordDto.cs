using System.ComponentModel.DataAnnotations;

namespace GunterBar.Application.DTOs.User;

public class ResetPasswordDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(10, MinimumLength = 4)]
    public string Code { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;
}
