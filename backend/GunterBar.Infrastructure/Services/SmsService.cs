using GunterBar.Application.Interfaces;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace GunterBar.Infrastructure.Services;

public class SmsService : ISmsService
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromNumber;

    public SmsService(IConfiguration configuration)
    {
        _accountSid = configuration["Twilio:AccountSid"] ?? throw new InvalidOperationException("Twilio AccountSid not configured");
        _authToken = configuration["Twilio:AuthToken"] ?? throw new InvalidOperationException("Twilio AuthToken not configured");
        _fromNumber = configuration["Twilio:FromNumber"] ?? throw new InvalidOperationException("Twilio FromNumber not configured");
    }

    public async Task<bool> SendSmsAsync(string to, string message)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_accountSid}:{_authToken}"))}");

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("To", to),
                new KeyValuePair<string, string>("From", _fromNumber),
                new KeyValuePair<string, string>("Body", message)
            });

            var response = await client.PostAsync($"https://api.twilio.com/2010-04-01/Accounts/{_accountSid}/Messages.json", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
