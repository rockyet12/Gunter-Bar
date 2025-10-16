using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Users;

public record AuthenticateUserRequest(UserCredentialsDto Credentials);

public class AuthenticateUserUseCase : UseCase<AuthenticateUserRequest, UserDto>
{
    private readonly IUserService _userService;

    public AuthenticateUserUseCase(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task<ApiResponse<UserDto>> ExecuteAsync(AuthenticateUserRequest request)
    {
        try
        {
            if (request.Credentials == null)
            {
                return ApiResponse<UserDto>.Fail("Credenciales no proporcionadas");
            }

            if (string.IsNullOrWhiteSpace(request.Credentials.Email))
            {
                return ApiResponse<UserDto>.Fail("El email es requerido");
            }

            if (string.IsNullOrWhiteSpace(request.Credentials.Password))
            {
                return ApiResponse<UserDto>.Fail("La contraseña es requerida");
            }

            return await _userService.AuthenticateAsync(request.Credentials);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al autenticar: {ex.Message}");
        }
    }
}
