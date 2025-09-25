namespace BarGunter.Application.DTOs;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }

    public LoginResponse() { }
    public LoginResponse(bool Success, string Message, string Token)
    {
        this.Success = Success;
        this.Message = Message;
        this.Token = Token;
    }
}