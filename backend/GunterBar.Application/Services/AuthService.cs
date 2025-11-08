using BCrypt.Net;
using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;

    public AuthService(IUserRepository userRepository, IJwtService jwtService, IEmailService emailService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _emailService = emailService;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
            {
                return ApiResponse<AuthResponseDto>.Fail("El email ya está registrado");
            }

            if (!Enum.TryParse<UserRole>(registerDto.Role, out var userRole))
            {
                userRole = UserRole.Customer; // default to Customer if invalid
            }

            var user = new User(
                registerDto.FirstName,
                registerDto.LastName ?? string.Empty,
                registerDto.Email,
                BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                userRole
            )
            {
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                ProfileImageUrl = registerDto.ProfileImageUrl
            };

            var createdUser = await _userRepository.CreateAsync(user);
            var token = _jwtService.GenerateToken(createdUser.Id, createdUser.Email, createdUser.Role.ToString());

            // Send welcome email with discount information
            try
            {
                await _emailService.SendWelcomeEmailAsync(createdUser.Email, createdUser.Name);
            }
            catch (Exception emailEx)
            {
                // Log email error but don't fail registration
                // TODO: Add proper logging here
                Console.WriteLine($"Failed to send welcome email: {emailEx.Message}");
            }

            var response = new AuthResponseDto
            {
                Token = token,
                RefreshToken = _jwtService.GenerateRefreshToken(createdUser.Id),
                User = new UserDto
                {
                    Id = createdUser.Id,
                    FirstName = createdUser.Name,
                    LastName = createdUser.LastName,
                    Email = createdUser.Email,
                    Role = createdUser.Role,
                    PhoneNumber = createdUser.PhoneNumber,
                    Address = createdUser.Address,
                    ProfileImageUrl = createdUser.ProfileImageUrl
                }
            };

            return ApiResponse<AuthResponseDto>.Succeed(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<AuthResponseDto>.Fail($"Error al registrar usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return ApiResponse<AuthResponseDto>.Fail("Email o contraseña incorrectos");
            }

            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());

            var response = new AuthResponseDto
            {
                Token = token,
                RefreshToken = _jwtService.GenerateRefreshToken(user.Id),
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address
                }
            };

            return ApiResponse<AuthResponseDto>.Succeed(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<AuthResponseDto>.Fail($"Error al iniciar sesión: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> GetProfileAsync(int userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<UserDto>.Fail("Usuario no encontrado");
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.Name,
                Email = user.Email,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return ApiResponse<UserDto>.Succeed(userDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al obtener perfil: {ex.Message}");
        }
    }

    public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            var userId = _jwtService.ValidateRefreshToken(refreshToken);
            if (userId == 0)
            {
                return ApiResponse<AuthResponseDto>.Fail("Token de actualización inválido");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<AuthResponseDto>.Fail("Usuario no encontrado");
            }

            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());
            var newRefreshToken = _jwtService.GenerateRefreshToken(user.Id);

            var response = new AuthResponseDto
            {
                Token = token,
                RefreshToken = newRefreshToken,
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address
                }
            };

            return ApiResponse<AuthResponseDto>.Succeed(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<AuthResponseDto>.Fail($"Error al renovar el token: {ex.Message}");
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

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                return ApiResponse<bool>.Fail("Contraseña actual incorrecta");
            }

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                return ApiResponse<bool>.Fail("La nueva contraseña debe tener al menos 6 caracteres");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return ApiResponse<bool>.Succeed(true, "Contraseña actualizada correctamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Fail($"Error al cambiar la contraseña: {ex.Message}");
        }
    }
}
