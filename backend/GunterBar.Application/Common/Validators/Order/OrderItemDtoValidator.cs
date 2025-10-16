using FluentValidation;
using GunterBar.Application.DTOs.Order;

namespace GunterBar.Application.Common.Validators.Order;

/// <summary>
/// Validador para los items de una orden.
/// </summary>
public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
{
    private const int MAX_QUANTITY = 10;
    
    public OrderItemDtoValidator()
    {
        RuleFor(x => x.DrinkId)
            .NotEmpty()
            .WithMessage("El ID de la bebida es obligatorio");

        RuleFor(x => x.DrinkName)
            .NotEmpty()
            .WithMessage("El nombre de la bebida es obligatorio")
            .MaximumLength(100)
            .WithMessage("El nombre de la bebida no puede exceder los 100 caracteres");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("La cantidad debe ser mayor que 0")
            .LessThanOrEqualTo(MAX_QUANTITY)
            .WithMessage($"La cantidad no puede ser mayor a {MAX_QUANTITY}");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("El precio unitario debe ser mayor que 0");
            
        RuleFor(x => x.Subtotal)
            .Equal(x => x.Quantity * x.UnitPrice)
            .WithMessage("El subtotal no coincide con cantidad * precio unitario");
    }
}
