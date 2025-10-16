using FluentValidation;
using GunterBar.Application.DTOs.Drinks;

namespace GunterBar.Application.Common.Validators.Drinks;

public class CreateDrinkDtoValidator : AbstractValidator<CreateDrinkDto>
{
    public CreateDrinkDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede tener más de 500 caracteres");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(2000).WithMessage("La URL de la imagen no puede tener más de 2000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));

        RuleFor(x => x.Category)
            .MaximumLength(50).WithMessage("La categoría no puede tener más de 50 caracteres");
    }
}
