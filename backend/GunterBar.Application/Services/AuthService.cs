using BCrypt.Net;
using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Application.Services;

// Implementación del servicio de autenticación
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
    {
        // Verificar si el email ya existe
        if (await _userRepository.EmailExistsAsync(registerDto.Email))
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "El email ya está registrado",
                Errors = { "Email already exists" }
            };
        }

        // Crear nuevo usuario
        var user = new User
        {
            Email = registerDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Role = UserRole.Cliente
        };

        var createdUser = await _userRepository.CreateAsync(user);

        // Generar token
        var token = _jwtService.GenerateToken(createdUser.Id, createdUser.Email, createdUser.Role.ToString());

        return new ApiResponse<AuthResponseDto>
        {
            Success = true,
            Message = "Usuario registrado exitosamente",
            Data = new AuthResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = new UserDto
                {
                    Id = createdUser.Id,
                    Email = createdUser.Email,
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    FullName = createdUser.FullName,
                    Role = createdUser.Role.ToString()
                }
            }
        };
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "Credenciales inválidas",
                Errors = { "Invalid credentials" }
            };
        }

        if (!user.IsActive)
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "Usuario inactivo",
                Errors = { "User is inactive" }
            };
        }

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());

        return new ApiResponse<AuthResponseDto>
        {
            Success = true,
            Message = "Login exitoso",
            Data = new AuthResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Role = user.Role.ToString()
                }
            }
        };
    }

    public async Task<ApiResponse<UserDto>> GetProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            return new ApiResponse<UserDto>
            {
                Success = false,
                Message = "Usuario no encontrado",
                Errors = { "User not found" }
            };
        }

        return new ApiResponse<UserDto>
        {
            Success = true,
            Message = "Perfil obtenido exitosamente",
            Data = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Role = user.Role.ToString()
            }
        };
    }
}
