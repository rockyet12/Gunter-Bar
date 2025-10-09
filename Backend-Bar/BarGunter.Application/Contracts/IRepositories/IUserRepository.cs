using BarGunter.Domain.Entities;

namespace BarGunter.Application.Contracts.IRepositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(int id, User user);
    Task<bool> DeleteAsync(int id);
    Task<bool> EmailExistsAsync(string email);
}

