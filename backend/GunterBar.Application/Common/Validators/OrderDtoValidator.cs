using FluentValidation;
using GunterBar.Application.DTOs.Order;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.Common.Validators.Order;

/// <summary>
/// Validador para las órdenes.
/// </summary>
public class OrderDtoValidator : AbstractValidator<OrderDto>
{
    private const int MAX_ITEMS = 20;

    public OrderDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .When(x => x.Id != 0)
            .WithMessage("ID de orden inválido");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El ID del usuario es obligatorio");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("El nombre del usuario es obligatorio")
            .MaximumLength(100)
            .WithMessage("El nombre del usuario no puede exceder los 100 caracteres");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("La orden debe tener al menos un item")
            .Must(x => x.Count <= MAX_ITEMS)
            .WithMessage($"La orden no puede tener más de {MAX_ITEMS} items");

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemDtoValidator());

        RuleFor(x => x.Total)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El total no puede ser negativo")
            .Must((order, total) => total == order.Items.Sum(i => i.Subtotal))
            .WithMessage("El total no coincide con la suma de los subtotales");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Estado de orden inválido");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("Las notas no pueden exceder los 500 caracteres");

        RuleFor(x => x.OrderDate)
            .NotEmpty()
            .WithMessage("La fecha de orden es obligatoria")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("La fecha de orden no puede ser futura");

        RuleFor(x => x.CompletedDate)
            .GreaterThan(x => x.OrderDate)
            .When(x => x.Status == OrderStatus.Completed && x.CompletedDate.HasValue)
            .WithMessage("La fecha de completado debe ser posterior a la fecha de orden");

        RuleFor(x => x.CancelledDate)
            .GreaterThan(x => x.OrderDate)
            .When(x => x.Status == OrderStatus.Cancelled && x.CancelledDate.HasValue)
            .WithMessage("La fecha de cancelación debe ser posterior a la fecha de orden");
    }
}
