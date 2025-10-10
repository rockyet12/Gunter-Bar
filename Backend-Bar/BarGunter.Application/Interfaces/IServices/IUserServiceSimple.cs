using BarGunter.Application.DTOs;
using BarGunter.Domain.Entities;

namespace BarGunter.Application.Interfaces.IServices;

public interface IUserServiceSimple
{
    Task<LoginResponse> TestLoginAsync();
}
