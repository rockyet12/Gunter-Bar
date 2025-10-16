using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using GunterBar.Application.Common.Models;

namespace GunterBar.Application.Common.Behaviors;

/// <summary>
/// Implementa el comportamiento de validación usando FluentValidation.
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IValidationBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : ApiResponse<object>, new()
{
    private readonly IValidator<TRequest>? _validator;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del comportamiento de validación.
    /// </summary>
    /// <param name="validator">El validador a utilizar. Puede ser nulo.</param>
    /// <param name="logger">El logger para registrar información de validación.</param>
    public ValidationBehavior(
        IValidator<TRequest>? validator = null,
        ILogger<ValidationBehavior<TRequest, TResponse>>? logger = null)
    {
        _validator = validator;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Valida una solicitud de forma asíncrona.
    /// </summary>
    /// <param name="request">La solicitud a validar.</param>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>Una respuesta API indicando el resultado de la validación.</returns>
    public async Task<TResponse> ValidateAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (_validator == null)
        {
            _logger.LogDebug("No hay validador configurado para {RequestType}", typeof(TRequest).Name);
            return new TResponse { Success = true };
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            _logger.LogDebug("Validación exitosa para {RequestType}", typeof(TRequest).Name);
            return new TResponse { Success = true };
        }

        _logger.LogWarning(
            "Validación fallida para {RequestType}. Errores: {Errors}",
            typeof(TRequest).Name,
            FormatValidationErrors(validationResult));

        var response = new TResponse
        {
            Success = false,
            Message = "Error de validación"
        };

        var groupedErrors = validationResult.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray());

        response.AddErrors(groupedErrors.SelectMany(x => x.Value));
        
        return response;
    }

    private static string FormatValidationErrors(ValidationResult validationResult)
    {
        return string.Join("; ", 
            validationResult.Errors.Select(e => 
                $"{e.PropertyName}: {e.ErrorMessage}"));
    }
}
}
