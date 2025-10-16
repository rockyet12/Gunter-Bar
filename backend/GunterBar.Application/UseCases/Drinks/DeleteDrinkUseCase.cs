using GunterBar.Application.Common.Models;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Drinks;

public record DeleteDrinkRequest(int DrinkId);

public class DeleteDrinkUseCase : UseCase<DeleteDrinkRequest, bool>
{
    private readonly IDrinkService _drinkService;

    public DeleteDrinkUseCase(IDrinkService drinkService)
    {
        _drinkService = drinkService;
    }

    protected override async Task<ApiResponse<bool>> ExecuteAsync(DeleteDrinkRequest request)
    {
        try
        {
            if (request.DrinkId <= 0)
            {
                return ApiResponse<bool>.Fail("ID de bebida inválido");
            }

            // Verificar si la bebida existe antes de intentar eliminarla
            var drink = await _drinkService.GetByIdAsync(request.DrinkId);
            if (!drink.Success || drink.Data == null)
            {
                return ApiResponse<bool>.Fail("La bebida no existe");
            }

            return await _drinkService.DeleteAsync(request.DrinkId);
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Fail($"Error al eliminar la bebida: {ex.Message}");
        }
    }
}
