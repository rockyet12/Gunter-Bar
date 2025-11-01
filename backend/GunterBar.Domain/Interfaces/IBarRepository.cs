using GunterBar.Domain.Entities;

namespace GunterBar.Domain.Interfaces;

// Contrato para repositorio de bares
public interface IBarRepository
{
    Task<Bar?> GetByIdAsync(int id);
    Task<Bar?> GetByOwnerIdAsync(int ownerId);
    Task<IEnumerable<Bar>> GetAllAsync();
    Task<IEnumerable<Bar>> GetActiveBarsAsync();
    Task<Bar> CreateAsync(Bar bar);
    Task<Bar> UpdateAsync(Bar bar);
    Task DeleteAsync(int id);
    Task<bool> BarExistsAsync(string name, int? excludeId = null);
}
