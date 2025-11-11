using System.Text.RegularExpressions;
using BCrypt.Net;
using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Bar;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IBarRepository _barRepository;
    private const int MaxLoginAttempts = 5;
    private const int MinPasswordLength = 6;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(30);
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public UserService(IUserRepository userRepository, IBarRepository barRepository)
    {
        _userRepository = userRepository;
        _barRepository = barRepository;
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.Name,
            LastName = user.LastName ?? string.Empty,
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            ProfileImageUrl = user.ProfileImageUrl,
            DeliveryDescription = user.DeliveryDescription,
            BirthDate = user.BirthDate,
            CreatedAt = user.CreatedAt,
            // Bar = user.Bar != null ? new BarDto
            // {
            //     Id = user.Bar.Id,
            //     Name = user.Bar.Name,
            //     Description = user.Bar.Description,
            //     OwnerId = user.Bar.OwnerId,
            //     OwnerName = user.Bar.Owner.Name,
            //     Address = user.Bar.Address,
            //     City = user.Bar.City,
            //     PostalCode = user.Bar.PostalCode,
            //     Country = user.Bar.Country,
            //     Latitude = user.Bar.Latitude,
            //     Longitude = user.Bar.Longitude,
            //     PhoneNumber = user.Bar.PhoneNumber,
            //     Email = user.Bar.Email,
            //     ImageUrl = user.Bar.ImageUrl,
            //     OpeningHours = user.Bar.OpeningHours,
            //     IsActive = user.Bar.IsActive,
            //     CreatedAt = user.Bar.CreatedAt,
            //     DrinksCount = user.Bar.Drinks.Count
            // } : null
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
            if (string.IsNullOrWhiteSpace(createUserDto.FirstName))
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

            // Crear el usuario
            var user = new User(
                createUserDto.FirstName.Trim(),
                createUserDto.LastName?.Trim() ?? string.Empty,
                createUserDto.Email.Trim().ToLower(),
                HashPassword(createUserDto.Password),
                createUserDto.Role)
            {
                PhoneNumber = createUserDto.PhoneNumber?.Trim(),
                Address = createUserDto.Address?.Trim(),
                BirthDate = createUserDto.BirthDate
            };

            var createdUser = await _userRepository.CreateAsync(user);

            // Si es vendor y tiene información del bar, crear el bar
            if (createUserDto.Role == UserRole.Vendor && createUserDto.BarInfo != null)
            {
                var bar = new Bar
                {
                    Name = createUserDto.BarInfo.Name.Trim(),
                    Description = createUserDto.BarInfo.Description?.Trim(),
                    OwnerId = createdUser.Id,
                    Address = createUserDto.BarInfo.Address?.Trim(),
                    City = createUserDto.BarInfo.City?.Trim(),
                    PostalCode = createUserDto.BarInfo.PostalCode?.Trim(),
                    Country = createUserDto.BarInfo.Country?.Trim(),
                    Latitude = createUserDto.BarInfo.Latitude,
                    Longitude = createUserDto.BarInfo.Longitude,
                    PhoneNumber = createUserDto.BarInfo.PhoneNumber?.Trim(),
                    Email = createUserDto.BarInfo.Email?.Trim().ToLower(),
                    ImageUrl = createUserDto.BarInfo.ImageUrl?.Trim(),
                    OpeningHours = createUserDto.BarInfo.OpeningHours?.Trim(),
                    IsActive = true
                };

                await _barRepository.CreateAsync(bar);
            }

            // Obtener el usuario con el bar incluido si fue creado
            var userWithBar = await _userRepository.GetByIdAsync(createdUser.Id);
            return ApiResponse<UserDto>.Succeed(MapToDto(userWithBar!), "Usuario creado exitosamente");
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
            user.LastName = updateUserDto.LastName;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.Address = updateUserDto.Address;
            user.DeliveryDescription = updateUserDto.DeliveryDescription;
            user.BirthDate = updateUserDto.BirthDate;
            user.Dni = updateUserDto.Dni;

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

    public async Task<ApiResponse<bool>> SetPasswordResetCodeAsync(string email, string code)
    {
        if (!IsValidEmail(email))
            return ApiResponse<bool>.Fail("Email inválido");
        if (string.IsNullOrWhiteSpace(code) || code.Length < 4)
            return ApiResponse<bool>.Fail("Código inválido");
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return ApiResponse<bool>.Fail("Usuario no encontrado");
        user.PasswordResetCode = code;
        user.PasswordResetCodeGeneratedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
        return ApiResponse<bool>.Succeed(true, "Código de recuperación guardado");
    }

    public async Task<ApiResponse<bool>> ResetPasswordWithCodeAsync(string email, string code, string newPassword)
    {
        if (!IsValidEmail(email))
            return ApiResponse<bool>.Fail("Email inválido");
        if (string.IsNullOrWhiteSpace(code) || code.Length < 4)
            return ApiResponse<bool>.Fail("Código inválido");
        if (!IsValidPassword(newPassword))
            return ApiResponse<bool>.Fail($"La contraseña debe tener al menos {MinPasswordLength} caracteres");
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return ApiResponse<bool>.Fail("Usuario no encontrado");
        if (user.PasswordResetCode == null || user.PasswordResetCode.ToUpper() != code.ToUpper())
            return ApiResponse<bool>.Fail("Código incorrecto");
        if (user.PasswordResetCodeGeneratedAt == null || (DateTime.UtcNow - user.PasswordResetCodeGeneratedAt.Value).TotalMinutes > 30)
            return ApiResponse<bool>.Fail("El código ha expirado");
        user.PasswordHash = HashPassword(newPassword);
        user.PasswordResetCode = null;
        user.PasswordResetCodeGeneratedAt = null;
        await _userRepository.UpdateAsync(user);
        return ApiResponse<bool>.Succeed(true, "Contraseña actualizada correctamente");
    }

    public async Task<ApiResponse<bool>> GenerateSmsVerificationCodeAsync(int userId, string phoneNumber)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return ApiResponse<bool>.Fail("Usuario no encontrado");
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return ApiResponse<bool>.Fail("Número de teléfono requerido");

        var code = GenerateRandomCode(6);
        user.SmsVerificationCode = code;
        user.SmsVerificationCodeGeneratedAt = DateTime.UtcNow;
        user.PhoneNumber = phoneNumber; // Actualizar el número de teléfono
        await _userRepository.UpdateAsync(user);
        return ApiResponse<bool>.Succeed(true, "Código generado correctamente");
    }

    public async Task<ApiResponse<bool>> VerifySmsCodeAsync(int userId, string code)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return ApiResponse<bool>.Fail("Usuario no encontrado");
        if (user.SmsVerificationCode == null || user.SmsVerificationCode != code)
            return ApiResponse<bool>.Fail("Código incorrecto");
        if (user.SmsVerificationCodeGeneratedAt == null || (DateTime.UtcNow - user.SmsVerificationCodeGeneratedAt.Value).TotalMinutes > 10)
            return ApiResponse<bool>.Fail("El código ha expirado");

        user.SmsVerificationCode = null;
        user.SmsVerificationCodeGeneratedAt = null;
        await _userRepository.UpdateAsync(user);
        return ApiResponse<bool>.Succeed(true, "Código verificado correctamente");
    }

    public async Task<ApiResponse<VendorProfileDto>> GetVendorProfileAsync(int vendorId)
    {
        try
        {
            var user = await _userRepository.GetByIdWithBarAsync(vendorId);
            if (user == null || user.Role != UserRole.Vendor)
            {
                return ApiResponse<VendorProfileDto>.Fail("Vendedor no encontrado");
            }

            var vendorProfile = new VendorProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ProfileImageUrl = user.ProfileImageUrl,
                Bar = user.Bar != null ? new BarDto
                {
                    Id = user.Bar.Id,
                    Name = user.Bar.Name,
                    Description = user.Bar.Description,
                    Address = user.Bar.Address,
                    City = user.Bar.City,
                    PostalCode = user.Bar.PostalCode,
                    Country = user.Bar.Country,
                    Latitude = user.Bar.Latitude,
                    Longitude = user.Bar.Longitude,
                    PhoneNumber = user.Bar.PhoneNumber,
                    Email = user.Bar.Email,
                    ImageUrl = user.Bar.ImageUrl,
                    OpeningHours = user.Bar.OpeningHours
                } : null
            };

            return ApiResponse<VendorProfileDto>.Succeed(vendorProfile);
        }
        catch (Exception ex)
        {
            return ApiResponse<VendorProfileDto>.Fail($"Error al obtener perfil del vendedor: {ex.Message}");
        }
    }

    private static string GenerateRandomCode(int length)
    {
        const string chars = "0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
