using GunterBar.Application.DTOs;

namespace GunterBar.Application.Interfaces;

// Servicio de autenticación
public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto);
    Task<ApiResponse<UserDto>> GetProfileAsync(int userId);
}

// Servicio de JWT
public interface IJwtService
{
    string GenerateToken(int userId, string email, string role);
    bool ValidateToken(string token);
    int GetUserIdFromToken(string token);
}
