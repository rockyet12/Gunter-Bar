using FluentValidation;
using GunterBar.Application.DTOs.Order;

namespace GunterBar.Application.Common.Validators.Orders;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("El ID del usuario es inválido");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Las notas no pueden tener más de 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
