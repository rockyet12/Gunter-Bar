namespace BarGunter.Application.DTOs;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public LoginRequest(string Email, string Password)
    {
        this.Email = Email;
        this.Password = Password;
    }                
}
