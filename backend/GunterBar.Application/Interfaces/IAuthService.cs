using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.User;

namespace GunterBar.Application.Interfaces;

// Servicio de autenticación
public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto);
    Task<ApiResponse<UserDto>> GetProfileAsync(int userId);
    Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(string refreshToken);
    Task<ApiResponse<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
}

// Servicio de JWT
public interface IJwtService
{
    string GenerateToken(int userId, string email, string role);
    bool ValidateToken(string token);
    int GetUserIdFromToken(string token);
    string GenerateRefreshToken(int userId);
    int ValidateRefreshToken(string refreshToken);
}
