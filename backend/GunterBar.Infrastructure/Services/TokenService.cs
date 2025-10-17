using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GunterBar.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _jwtSecret;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly int _jwtExpirationMinutes;
    private readonly int _refreshTokenExpirationDays;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        
        _jwtSecret = _configuration["Jwt:Secret"] ?? 
            throw new InvalidOperationException("JWT Secret no está configurado");
        _jwtIssuer = _configuration["Jwt:Issuer"] ?? "GunterBar";
        _jwtAudience = _configuration["Jwt:Audience"] ?? "GunterBarAPI";
        
        if (!int.TryParse(_configuration["Jwt:ExpirationMinutes"], out _jwtExpirationMinutes))
            _jwtExpirationMinutes = 60; // 1 hora por defecto
            
        if (!int.TryParse(_configuration["Jwt:RefreshTokenExpirationDays"], out _refreshTokenExpirationDays))
            _refreshTokenExpirationDays = 7; // 7 días por defecto
    }

    public string GenerateToken(UserDto user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("El email del usuario es requerido", nameof(user));

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSecret);
        
        var claims = new List<Claim>
        {
            new Claim("uid", user.Id.ToString()),
            new Claim("email", user.Email),
            new Claim("role", user.Role.ToString()),
            new Claim("name", user.Name ?? string.Empty),
            new Claim("jti", Guid.NewGuid().ToString()),
            new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        if (!string.IsNullOrEmpty(user.PhoneNumber))
            claims.Add(new Claim("phone_number", user.PhoneNumber));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
            Issuer = _jwtIssuer,
            Audience = _jwtAudience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            ),
            NotBefore = DateTime.UtcNow
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token no puede estar vacío", nameof(token));

        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(token))
            throw new ArgumentException("Token inválido", nameof(token));

        var key = Encoding.UTF8.GetBytes(_jwtSecret);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _jwtIssuer,
            ValidateAudience = true,
            ValidAudience = _jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            // Validación adicional del algoritmo
            if (validatedToken is JwtSecurityToken jwtToken)
            {
                var result = jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, 
                    StringComparison.InvariantCultureIgnoreCase);

                if (!result)
                    throw new SecurityTokenException("Algoritmo de firma inválido");
            }

            return principal;
        }
        catch (SecurityTokenExpiredException)
        {
            throw new SecurityTokenExpiredException("El token ha expirado");
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            throw new SecurityTokenInvalidSignatureException("La firma del token es inválida");
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException($"Error al validar el token: {ex.Message}");
        }
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64]; // Aumentado a 64 bytes para mayor seguridad
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public DateTime GetRefreshTokenExpiryTime()
    {
        return DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);
    }
}
