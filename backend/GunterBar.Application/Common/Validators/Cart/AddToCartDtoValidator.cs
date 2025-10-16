using FluentValidation;
using GunterBar.Application.DTOs.Cart;

namespace GunterBar.Application.Common.Validators.Cart;

public class AddToCartDtoValidator : AbstractValidator<AddToCartDto>
{
    private const int MaxQuantityPerItem = 10;

    public AddToCartDtoValidator()
    {
        RuleFor(x => x.DrinkId)
            .GreaterThan(0).WithMessage("El ID de la bebida es inválido");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0")
            .LessThanOrEqualTo(MaxQuantityPerItem)
            .WithMessage($"No puede agregar más de {MaxQuantityPerItem} unidades del mismo producto");
    }
}
