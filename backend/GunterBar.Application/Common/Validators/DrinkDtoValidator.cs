using FluentValidation;
using GunterBar.Application.DTOs.Drinks;

namespace GunterBar.Application.Common.Validators;

public class DrinkDtoValidator : AbstractValidator<DrinkDto>
{
    public DrinkDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede tener más de 500 caracteres");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor que 0");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("El tipo de bebida es obligatorio");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo");
    }
}
