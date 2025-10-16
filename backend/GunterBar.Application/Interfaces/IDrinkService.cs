using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Drinks;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.Interfaces;

// Servicio de bebidas
public interface IDrinkService
{
    Task<ApiResponse<IEnumerable<DrinkDto>>> GetAllAsync();
    Task<ApiResponse<IEnumerable<DrinkDto>>> GetByTypeAsync(DrinkType type);
    Task<ApiResponse<DrinkDto>> GetByIdAsync(int id);
    Task<ApiResponse<DrinkDto>> CreateAsync(CreateDrinkDto createDrinkDto);
    Task<ApiResponse<DrinkDto>> UpdateAsync(int id, UpdateDrinkDto updateDrinkDto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}
