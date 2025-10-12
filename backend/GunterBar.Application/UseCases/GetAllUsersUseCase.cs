using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;

namespace GunterBar.Application.UseCases;

public class GetAllUsersUseCase
{
    private readonly IUserService _userService;

    public GetAllUsersUseCase(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IEnumerable<UserDto>> ExecuteAsync()
    {
        return await _userService.GetAllUsersAsync();
    }
}
