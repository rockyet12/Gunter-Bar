using System.Text.Json.Serialization;

namespace GunterBar.Application.Common.Models;

/// <summary>
/// Define el contrato para todas las respuestas API en la aplicación.
/// </summary>
public interface IApiResponse
{
    /// <summary>
    /// Obtiene o establece un valor que indica si la operación fue exitosa.
    /// </summary>
    bool Success { get; set; }

    /// <summary>
    /// Obtiene o establece el mensaje asociado con la respuesta.
    /// </summary>
    string Message { get; set; }

    /// <summary>
    /// Obtiene o establece la lista de errores que ocurrieron durante la operación.
    /// </summary>
    List<string> Errors { get; set; }

    /// <summary>
    /// Obtiene un valor que indica si hay errores.
    /// </summary>
    [JsonIgnore]
    bool HasErrors { get; }

    /// <summary>
    /// Agrega un único error a la colección de errores.
    /// </summary>
    void AddError(string error);

    /// <summary>
    /// Agrega múltiples errores a la colección de errores.
    /// </summary>
    void AddErrors(IEnumerable<string> errors);

    /// <summary>
    /// Limpia todos los errores de la colección de errores.
    /// </summary>
    void ClearErrors();
}
