using GunterBar.Application.Common.Models;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Users;

public record DeleteUserRequest(int UserId);

public class DeleteUserUseCase : UseCase<DeleteUserRequest, bool>
{
    private readonly IUserService _userService;

    public DeleteUserUseCase(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task<ApiResponse<bool>> ExecuteAsync(DeleteUserRequest request)
    {
        try
        {
            if (request.UserId <= 0)
            {
                return ApiResponse<bool>.Fail("ID de usuario inválido");
            }

            return await _userService.DeleteAsync(request.UserId);
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Fail($"Error al eliminar el usuario: {ex.Message}");
        }
    }
}
