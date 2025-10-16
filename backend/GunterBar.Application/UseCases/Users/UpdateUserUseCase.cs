using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Users;

public record UpdateUserRequest(int UserId, UpdateUserDto UserData);

public class UpdateUserUseCase : UseCase<UpdateUserRequest, UserDto>
{
    private readonly IUserService _userService;

    public UpdateUserUseCase(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task<ApiResponse<UserDto>> ExecuteAsync(UpdateUserRequest request)
    {
        try
        {
            if (request.UserId <= 0)
            {
                return ApiResponse<UserDto>.Fail("ID de usuario inválido");
            }

            if (request.UserData == null)
            {
                return ApiResponse<UserDto>.Fail("Datos de actualización no proporcionados");
            }

            if (string.IsNullOrWhiteSpace(request.UserData.Name))
            {
                return ApiResponse<UserDto>.Fail("El nombre es requerido");
            }

            return await _userService.UpdateAsync(request.UserId, request.UserData);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al actualizar el usuario: {ex.Message}");
        }
    }
}
