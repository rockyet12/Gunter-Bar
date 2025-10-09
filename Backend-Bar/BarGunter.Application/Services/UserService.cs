using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Application.DTOs;
using BarGunter.Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BarGunter.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly string _jwtSecret = "TucodigodeseguridadWAZAAAAAAA!!"; // En producción, usar configuración

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<User> CreateAsync(User user)
    {
        // Hash the password before saving
        user.Password = HashPassword(user.Password);
        return await _userRepository.CreateAsync(user);
    }

    public async Task<User?> UpdateAsync(int id, User user)
    {
        return await _userRepository.UpdateAsync(id, user);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _userRepository.DeleteAsync(id);
    }

    public async Task<BarGunter.Application.DTOs.LoginResponse> LoginAsync(BarGunter.Application.DTOs.LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null || !VerifyPassword(request.Password, user.Password))
        {
            return new BarGunter.Application.DTOs.LoginResponse
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        if (!user.IsActive)
        {
            return new BarGunter.Application.DTOs.LoginResponse
            {
                Success = false,
                Message = "User account is inactive"
            };
        }

        var tokenResponse = GenerateJwtToken(user);

    return new BarGunter.Application.DTOs.LoginResponse
        {
            Success = true,
            Message = "Login successful",
            TokenInfo = tokenResponse,
        User = new BarGunter.Application.DTOs.UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            }
        };
    }

    public async Task<BarGunter.Application.DTOs.LoginResponse> RegisterAsync(BarGunter.Application.DTOs.RegisterRequest request)
    {
        // Check if email already exists
        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            return new BarGunter.Application.DTOs.LoginResponse
            {
                Success = false,
                Message = "Email already exists"
            };
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = HashPassword(request.Password),
            Gender = request.Gender,
            Role = "Customer"
        };

    var createdUser = await _userRepository.CreateAsync(user);
    var tokenResponse = GenerateJwtToken(createdUser);

    return new BarGunter.Application.DTOs.LoginResponse
        {
            Success = true,
            Message = "Registration successful",
            TokenInfo = tokenResponse,
            User = new BarGunter.Application.DTOs.UserDto
            {
                UserId = createdUser.UserId,
                Name = createdUser.Name,
                Email = createdUser.Email,
                Role = createdUser.Role
            }
        };
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hashedPassword;
    }

    private BarGunter.Application.DTOs.TokenResponse GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var expiration = DateTime.UtcNow.AddDays(7);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = expiration,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        
    return new BarGunter.Application.DTOs.TokenResponse(tokenString, expiration, "Token generated successfully");
    }
}

