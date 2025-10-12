using System.Text.Json.Serialization;

namespace GunterBar.Application.Common.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; } = new();

    [JsonIgnore]
    public bool HasErrors => Errors?.Any() == true;

    public static ApiResponse<T> Succeed(T data, string message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message ?? "Operación exitosa",
            Data = data
        };
    }

    public static ApiResponse<T> Fail(string message, List<string> errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
