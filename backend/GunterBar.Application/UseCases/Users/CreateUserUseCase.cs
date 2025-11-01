using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Users;

public record CreateUserRequest(CreateUserDto UserData);

public class CreateUserUseCase : UseCase<CreateUserRequest, UserDto>
{
    private readonly IUserService _userService;

    public CreateUserUseCase(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task<ApiResponse<UserDto>> ExecuteAsync(CreateUserRequest request)
    {
        try
        {
            if (request.UserData == null)
            {
                return ApiResponse<UserDto>.Fail("Datos de usuario no proporcionados");
            }

            if (string.IsNullOrWhiteSpace(request.UserData.FirstName))
            {
                return ApiResponse<UserDto>.Fail("El nombre es requerido");
            }

            if (string.IsNullOrWhiteSpace(request.UserData.LastName))
            {
                return ApiResponse<UserDto>.Fail("El apellido es requerido");
            }

            if (string.IsNullOrWhiteSpace(request.UserData.Email))
            {
                return ApiResponse<UserDto>.Fail("El email es requerido");
            }

            if (string.IsNullOrWhiteSpace(request.UserData.Password))
            {
                return ApiResponse<UserDto>.Fail("La contraseña es requerida");
            }

            return await _userService.CreateAsync(request.UserData);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al crear el usuario: {ex.Message}");
        }
    }
}
