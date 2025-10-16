using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Drinks;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Drinks;

public record UpdateDrinkRequest(int DrinkId, UpdateDrinkDto DrinkData);

public class UpdateDrinkUseCase : UseCase<UpdateDrinkRequest, DrinkDto>
{
    private readonly IDrinkService _drinkService;

    public UpdateDrinkUseCase(IDrinkService drinkService)
    {
        _drinkService = drinkService;
    }

    protected override async Task<ApiResponse<DrinkDto>> ExecuteAsync(UpdateDrinkRequest request)
    {
        try
        {
            if (request.DrinkId <= 0)
            {
                return ApiResponse<DrinkDto>.Fail("ID de bebida inválido");
            }

            if (request.DrinkData == null)
            {
                return ApiResponse<DrinkDto>.Fail("Los datos de actualización son requeridos");
            }

            // Validar los datos de la bebida
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

            // Verificar si la bebida existe
            var existingDrink = await _drinkService.GetByIdAsync(request.DrinkId);
            if (!existingDrink.Success || existingDrink.Data == null)
            {
                return ApiResponse<DrinkDto>.Fail($"Bebida con ID {request.DrinkId} no encontrada");
            }

            return await _drinkService.UpdateAsync(request.DrinkId, request.DrinkData);
        }
        catch (Exception ex)
        {
            return ApiResponse<DrinkDto>.Fail($"Error al actualizar la bebida: {ex.Message}");
        }
    }
}
