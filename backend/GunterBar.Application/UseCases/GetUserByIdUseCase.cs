using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;

namespace GunterBar.Application.UseCases;

public class GetUserByIdUseCase
{
    private readonly IUserService _userService;

    public GetUserByIdUseCase(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserDto> ExecuteAsync(Guid userId)
    {
        return await _userService.GetUserByIdAsync(userId);
    }
}
