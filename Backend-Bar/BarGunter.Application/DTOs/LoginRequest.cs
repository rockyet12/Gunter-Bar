namespace BarGunter.Application.DTOs;

public class LoginRequest
{
    /// <summary>
    /// Email del usuario. Requerido y debe ser formato email válido.
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Contraseña del usuario. Requerido y mínimo 6 caracteres.
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    public LoginRequest(string Email, string Password)
    {
        this.Email = Email;
        this.Password = Password;
    }
}
