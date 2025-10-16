using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.UseCases.Users;

public record UpdateUserRoleRequest(int UserId, UserRole NewRole);

public class UpdateUserRoleUseCase : UseCase<UpdateUserRoleRequest, UserDto>
{
    private readonly IUserService _userService;

    public UpdateUserRoleUseCase(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task<ApiResponse<UserDto>> ExecuteAsync(UpdateUserRoleRequest request)
    {
        try
        {
            if (request.UserId <= 0)
            {
                return ApiResponse<UserDto>.Fail("ID de usuario inválido");
            }

            return await _userService.UpdateRoleAsync(request.UserId, request.NewRole);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al actualizar el rol del usuario: {ex.Message}");
        }
    }
}
