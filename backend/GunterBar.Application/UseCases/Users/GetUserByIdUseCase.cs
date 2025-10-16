using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Users;

public record GetUserByIdRequest(int UserId);

public class GetUserByIdUseCase : UseCase<GetUserByIdRequest, UserDto>
{
    private readonly IUserService _userService;

    public GetUserByIdUseCase(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task<ApiResponse<UserDto>> ExecuteAsync(GetUserByIdRequest request)
    {
        try
        {
            if (request.UserId <= 0)
            {
                return ApiResponse<UserDto>.Fail("ID de usuario inválido");
            }

            return await _userService.GetByIdAsync(request.UserId);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail($"Error al obtener el usuario: {ex.Message}");
        }
    }
}
