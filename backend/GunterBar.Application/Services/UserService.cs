using System.Text.RegularExpressions;
using BCrypt.Net;
using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private const int MaxLoginAttempts = 5;
    private const int MinPasswordLength = 6;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(30);
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            ProfileImageUrl = user.ProfileImageUrl,
            DeliveryDescription = user.DeliveryDescription,
            BirthDate = user.BirthDate
        };
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool ValidatePassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    private static bool IsValidEmail(string email)
    {
        return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email);
    }

    private static bool IsValidPassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && password.Length >= MinPasswordLength;
    }

    public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = users.Select(MapToDto);
            return ApiResponse<IEnumerable<UserDto>>.Succeed(userDtos, "Usuarios obtenidos exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<UserDto>>.Fail($"Error al obtener los usuarios: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> GetByIdAsync(int userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<UserDto>.Fail("Usuario no encontrado");
            }

            return ApiResponse<UserDto>.Succeed(MapToDto(user), "Usuario obtenido exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al obtener el usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> GetByEmailAsync(string email)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return ApiResponse<UserDto>.Fail("Usuario no encontrado");
            }

            return ApiResponse<UserDto>.Succeed(MapToDto(user), "Usuario obtenido exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al obtener el usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> CreateAsync(CreateUserDto createUserDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(createUserDto.Name))
            {
                return ApiResponse<UserDto>.Fail("El nombre es requerido");
            }

            if (!IsValidEmail(createUserDto.Email))
            {
                return ApiResponse<UserDto>.Fail("El formato del email es inválido");
            }

            if (!IsValidPassword(createUserDto.Password))
            {
                return ApiResponse<UserDto>.Fail($"La contraseña debe tener al menos {MinPasswordLength} caracteres");
            }

            // Validar si el email ya existe
            var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
            if (existingUser != null)
            {
                return ApiResponse<UserDto>.Fail("El email ya está registrado");
            }

            var user = new User(
                createUserDto.Name.Trim(),
                createUserDto.Email.Trim().ToLower(),
                HashPassword(createUserDto.Password))
            {
                PhoneNumber = createUserDto.PhoneNumber?.Trim(),
                Address = createUserDto.Address?.Trim()
            };

            var createdUser = await _userRepository.CreateAsync(user);
            return ApiResponse<UserDto>.Succeed(MapToDto(createdUser), "Usuario creado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al crear el usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> UpdateAsync(int userId, UpdateUserDto updateUserDto)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<UserDto>.Fail("Usuario no encontrado");
            }

            user.Name = updateUserDto.Name;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.Address = updateUserDto.Address;
            user.DeliveryDescription = updateUserDto.DeliveryDescription;
            user.BirthDate = updateUserDto.BirthDate;

            var updatedUser = await _userRepository.UpdateAsync(user);
            return ApiResponse<UserDto>.Succeed(MapToDto(updatedUser), "Usuario actualizado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al actualizar el usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.Fail("Usuario no encontrado");
            }

            await _userRepository.DeleteAsync(userId);
            return ApiResponse<bool>.Succeed(true, "Usuario eliminado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Fail($"Error al eliminar el usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> UpdateRoleAsync(int userId, UserRole newRole)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<UserDto>.Fail("Usuario no encontrado");
            }

            user.Role = newRole;
            var updatedUser = await _userRepository.UpdateAsync(user);
            return ApiResponse<UserDto>.Succeed(MapToDto(updatedUser), "Rol actualizado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al actualizar el rol: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> AuthenticateAsync(UserCredentialsDto credentials)
    {
        try
        {
            if (!IsValidEmail(credentials.Email))
            {
                return ApiResponse<UserDto>.Fail("Formato de email inválido");
            }

            var user = await _userRepository.GetByEmailAsync(credentials.Email);
            if (user == null)
            {
                return ApiResponse<UserDto>.Fail("Credenciales inválidas");
            }

            // Verificar si la cuenta está bloqueada
            if (user.LoginAttempts >= MaxLoginAttempts && 
                user.LastLoginAttempt.HasValue &&
                user.LastLoginAttempt.Value.AddMinutes(LockoutDuration.TotalMinutes) > DateTime.UtcNow)
            {
                var remainingLockoutTime = user.LastLoginAttempt.Value.AddMinutes(LockoutDuration.TotalMinutes) - DateTime.UtcNow;
                return ApiResponse<UserDto>.Fail($"Cuenta bloqueada. Intente nuevamente en {remainingLockoutTime.Minutes} minutos");
            }

            if (!ValidatePassword(credentials.Password, user.PasswordHash))
            {
                user.LoginAttempts++;
                user.LastLoginAttempt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                if (user.LoginAttempts >= MaxLoginAttempts)
                {
                    return ApiResponse<UserDto>.Fail($"Cuenta bloqueada por múltiples intentos fallidos. Intente nuevamente en {LockoutDuration.Minutes} minutos");
                }

                return ApiResponse<UserDto>.Fail($"Credenciales inválidas. Intentos restantes: {MaxLoginAttempts - user.LoginAttempts}");
            }

            // Reset login attempts on successful login
            user.LoginAttempts = 0;
            user.LastLoginAttempt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return ApiResponse<UserDto>.Succeed(MapToDto(user), "Autenticación exitosa");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al autenticar: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.Fail("Usuario no encontrado");
            }

            if (!ValidatePassword(currentPassword, user.PasswordHash))
            {
                return ApiResponse<bool>.Fail("Contraseña actual incorrecta");
            }

            if (!IsValidPassword(newPassword))
            {
                return ApiResponse<bool>.Fail($"La nueva contraseña debe tener al menos {MinPasswordLength} caracteres");
            }

            if (ValidatePassword(newPassword, user.PasswordHash))
            {
                return ApiResponse<bool>.Fail("La nueva contraseña debe ser diferente a la actual");
            }

            user.PasswordHash = HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return ApiResponse<bool>.Succeed(true, "Contraseña actualizada exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Fail($"Error al cambiar la contraseña: {ex.Message}");
        }
    }
}
