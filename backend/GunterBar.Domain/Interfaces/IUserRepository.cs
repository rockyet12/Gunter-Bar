using GunterBar.Domain.Entities;

namespace GunterBar.Domain.Interfaces;

// Contrato para repositorio de usuarios
public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByIdWithBarAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<bool> EmailExistsAsync(string email);
}
