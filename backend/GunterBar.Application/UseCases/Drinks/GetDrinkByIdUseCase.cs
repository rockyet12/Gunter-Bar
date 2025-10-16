using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Drinks;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Drinks;

public record GetDrinkByIdRequest(int DrinkId);

public class GetDrinkByIdUseCase : UseCase<GetDrinkByIdRequest, DrinkDto>
{
    private readonly IDrinkService _drinkService;

    public GetDrinkByIdUseCase(IDrinkService drinkService)
    {
        _drinkService = drinkService;
    }

    protected override async Task<ApiResponse<DrinkDto>> ExecuteAsync(GetDrinkByIdRequest request)
    {
        try
        {
            if (request.DrinkId <= 0)
            {
                return ApiResponse<DrinkDto>.Fail("ID de bebida inválido");
            }

            return await _drinkService.GetByIdAsync(request.DrinkId);
        }
        catch (Exception ex)
        {
            return ApiResponse<DrinkDto>.Fail($"Error al obtener la bebida: {ex.Message}");
        }
    }
}
