using System.Security.Claims;
using GunterBar.Application.DTOs.User;

namespace GunterBar.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(UserDto user);
    ClaimsPrincipal ValidateToken(string token);
    string GenerateRefreshToken();
}
