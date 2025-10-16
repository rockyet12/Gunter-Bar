using GunterBar.Application.Common.Models;

namespace GunterBar.Application.Common.Behaviors;

/// <summary>
/// Define un comportamiento de validación que puede ser usado para validar solicitudes antes de ser procesadas.
/// </summary>
/// <typeparam name="TRequest">El tipo de solicitud a validar. Debe ser una clase.</typeparam>
/// <typeparam name="TResponse">El tipo de respuesta a retornar. Debe ser un ApiResponse con un tipo específico.</typeparam>
public interface IValidationBehavior<in TRequest, TResponse> 
    where TRequest : class 
    where TResponse : ApiResponse<object>, new()
{
    /// <summary>
    /// Validates the given request and returns a response indicating success or failure.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>
    /// A response object with Success=true if validation passes,
    /// or Success=false with error details if validation fails.
    /// </returns>
    Task<TResponse> ValidateAsync(TRequest request, CancellationToken cancellationToken = default);
}
