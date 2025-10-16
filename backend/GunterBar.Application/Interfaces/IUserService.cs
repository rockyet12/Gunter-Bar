using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.User;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.Interfaces;

public interface IUserService
{
    // Queries
    Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
    Task<ApiResponse<UserDto>> GetByIdAsync(int userId);
    Task<ApiResponse<UserDto>> GetByEmailAsync(string email);

    // Commands
    Task<ApiResponse<UserDto>> CreateAsync(CreateUserDto createUserDto);
    Task<ApiResponse<UserDto>> UpdateAsync(int userId, UpdateUserDto updateUserDto);
    Task<ApiResponse<bool>> DeleteAsync(int userId);
    Task<ApiResponse<UserDto>> UpdateRoleAsync(int userId, UserRole newRole);
    
    // Auth
    Task<ApiResponse<UserDto>> AuthenticateAsync(UserCredentialsDto credentials);
    Task<ApiResponse<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
}
