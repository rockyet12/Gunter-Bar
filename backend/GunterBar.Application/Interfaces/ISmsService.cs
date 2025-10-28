namespace GunterBar.Application.Interfaces;

public interface ISmsService
{
    Task<bool> SendSmsAsync(string to, string message);
}
