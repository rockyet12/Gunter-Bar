namespace BarGunter.Application.DTOs;

public class TokenResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string Message { get; set; }
}
