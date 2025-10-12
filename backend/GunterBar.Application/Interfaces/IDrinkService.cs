using GunterBar.Application.DTOs;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.Interfaces;

// Servicio de bebidas
public interface IDrinkService
{
    Task<ApiResponse<IEnumerable<DrinkDto>>> GetAllAsync();
    Task<ApiResponse<IEnumerable<DrinkDto>>> GetByTypeAsync(DrinkType type);
    Task<ApiResponse<DrinkDto>> GetByIdAsync(int id);
    Task<ApiResponse<DrinkDto>> CreateAsync(CreateDrinkDto createDrinkDto);
    Task<ApiResponse<DrinkDto>> UpdateAsync(int id, CreateDrinkDto updateDrinkDto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}
