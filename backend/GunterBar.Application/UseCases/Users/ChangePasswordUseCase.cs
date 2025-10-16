using GunterBar.Application.Common.Models;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Users;

public record ChangePasswordRequest(int UserId, string CurrentPassword, string NewPassword);

public class ChangePasswordUseCase : UseCase<ChangePasswordRequest, bool>
{
    private readonly IUserService _userService;

    public ChangePasswordUseCase(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task<ApiResponse<bool>> ExecuteAsync(ChangePasswordRequest request)
    {
        try
        {
            if (request.UserId <= 0)
            {
                return ApiResponse<bool>.Fail("ID de usuario inválido");
            }

            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
            {
                return ApiResponse<bool>.Fail("La contraseña actual es requerida");
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return ApiResponse<bool>.Fail("La nueva contraseña es requerida");
            }

            if (request.NewPassword.Length < 6)
            {
                return ApiResponse<bool>.Fail("La nueva contraseña debe tener al menos 6 caracteres");
            }

            if (request.CurrentPassword == request.NewPassword)
            {
                return ApiResponse<bool>.Fail("La nueva contraseña debe ser diferente a la actual");
            }

            return await _userService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword);
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Fail($"Error al cambiar la contraseña: {ex.Message}");
        }
    }
}
