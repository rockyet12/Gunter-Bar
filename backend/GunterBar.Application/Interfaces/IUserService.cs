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
    /// <summary>
    /// Guarda el código de recuperación de contraseña para el usuario
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <param name="code">Código de recuperación</param>
    /// <returns>Resultado de la operación</returns>
    Task<ApiResponse<bool>> SetPasswordResetCodeAsync(string email, string code);
    /// <summary>
    /// Restablece la contraseña usando email y código de seguridad
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <param name="code">Código de recuperación</param>
    /// <param name="newPassword">Nueva contraseña</param>
    /// <returns>Resultado de la operación</returns>
    Task<ApiResponse<bool>> ResetPasswordWithCodeAsync(string email, string code, string newPassword);
    
    // SMS Verification
    Task<ApiResponse<bool>> GenerateSmsVerificationCodeAsync(int userId, string phoneNumber);
    Task<ApiResponse<bool>> VerifySmsCodeAsync(int userId, string code);
}
