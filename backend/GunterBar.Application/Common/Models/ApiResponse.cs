using System.Text.Json.Serialization;

namespace GunterBar.Application.Common.Models;

/// <summary>
/// Representa una respuesta API estandarizada con datos de tipo T.
/// </summary>
/// <typeparam name="T">El tipo de datos contenido en la respuesta.</typeparam>
public class ApiResponse<T> : IApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    [JsonIgnore]
    public bool HasErrors => Errors?.Any() == true;

    public void AddError(string error)
    {
        if (string.IsNullOrWhiteSpace(error)) return;
        
        Errors ??= new List<string>();
        Errors.Add(error);
        Success = false;
    }

    public void AddErrors(IEnumerable<string> errors)
    {
        if (errors == null) return;

        Errors ??= new List<string>();
        foreach (var error in errors.Where(e => !string.IsNullOrWhiteSpace(e)))
        {
            Errors.Add(error);
        }
        if (Errors.Any())
        {
            Success = false;
        }
    }

    public void ClearErrors()
    {
        Errors?.Clear();
    }

    public static ApiResponse<T> Succeed(T data, string message = "")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message ?? "Operación exitosa",
            Data = data
        };
    }

    public static ApiResponse<T> Fail(string message, string error)
    {
        var response = new ApiResponse<T>
        {
            Success = false,
            Message = message
        };
        response.AddError(error);
        return response;
    }

    public static ApiResponse<T> Fail(string message, IEnumerable<string> errors)
    {
        var response = new ApiResponse<T>
        {
            Success = false,
            Message = message
        };
        response.AddErrors(errors);
        return response;
    }

    public static ApiResponse<T> Fail(string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message
        };
    }
}
