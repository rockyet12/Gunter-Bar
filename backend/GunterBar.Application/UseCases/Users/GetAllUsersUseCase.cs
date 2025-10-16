using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Users;

public record GetAllUsersRequest;

public class GetAllUsersUseCase : UseCase<GetAllUsersRequest, IEnumerable<UserDto>>
{
    private readonly IUserService _userService;

    public GetAllUsersUseCase(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task<ApiResponse<IEnumerable<UserDto>>> ExecuteAsync(GetAllUsersRequest request)
    {
        try
        {
            return await _userService.GetAllUsersAsync();
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<UserDto>>.Fail($"Error al obtener los usuarios: {ex.Message}");
        }
    }
}
