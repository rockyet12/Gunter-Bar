using System.Runtime.CompilerServices;

namespace BarGunter.Application.DTOs;

public class TokenResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string Message { get; set; }
    
    public TokenResponse(string Token, DateTime Expiration, string Message)
    {
        this.Token = Token;
        this.Expiration = Expiration;
        this.Message = Message;
    }
}
