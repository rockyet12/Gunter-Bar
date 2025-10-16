using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Drinks;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Drinks;

public record CreateDrinkRequest(CreateDrinkDto DrinkData);

public class CreateDrinkUseCase : UseCase<CreateDrinkRequest, DrinkDto>
{
    private readonly IDrinkService _drinkService;

    public CreateDrinkUseCase(IDrinkService drinkService)
    {
        _drinkService = drinkService;
    }

    protected override async Task<ApiResponse<DrinkDto>> ExecuteAsync(CreateDrinkRequest request)
    {
        if (request.DrinkData == null)
        {
            return ApiResponse<DrinkDto>.Fail("Los datos de la bebida son requeridos");
        }

        if (string.IsNullOrWhiteSpace(request.DrinkData.Name))
        {
            return ApiResponse<DrinkDto>.Fail("El nombre es requerido");
        }

        if (request.DrinkData.Price <= 0)
        {
            return ApiResponse<DrinkDto>.Fail("El precio debe ser mayor a 0");
        }

        if (request.DrinkData.Stock < 0)
        {
            return ApiResponse<DrinkDto>.Fail("El stock no puede ser negativo");
        }

        return await _drinkService.CreateAsync(request.DrinkData);
    }
}
