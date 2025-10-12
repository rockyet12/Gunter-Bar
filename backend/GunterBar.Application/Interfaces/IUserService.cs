using GunterBar.Application.DTOs;

namespace GunterBar.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
}
