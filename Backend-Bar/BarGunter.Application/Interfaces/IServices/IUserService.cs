using BarGunter.Application.DTOs;
using BarGunter.Domain.Entities;

namespace BarGunter.Application.Contracts.IServices;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(int id, User user);
    Task<bool> DeleteAsync(int id);
    Task<BarGunter.Application.DTOs.LoginResponse> LoginAsync(BarGunter.Application.DTOs.LoginRequest request);
    Task<BarGunter.Application.DTOs.LoginResponse> RegisterAsync(BarGunter.Application.DTOs.RegisterRequest request);
}

