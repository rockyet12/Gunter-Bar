using BarGunter.Domain.Entities;
using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.DTOs;
using BarGunter.Application.Contracts.IServices;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
namespace BarGunter.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<List<Usuario>> GetAllUsuarios()
    {
        return await _usuarioRepository.GetAllUsuarios();
    }

    public async Task<Usuario> GetUsuarioById(int id)
    {
        return await _usuarioRepository.GetUsuarioById(id);
    }
    
    public async Task<int> AddUsuario(Usuario usuario)
    {
        return await _usuarioRepository.AddUsuario(usuario);
    }

    public async Task<bool> UpdateUsuario(Usuario usuario)
    {
        return await _usuarioRepository.UpdateUsuario(usuario);
    }

    public async Task<bool> DeleteUsuario(int id)
    {
        return await _usuarioRepository.DeleteUsuario(id);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        // En una aplicación real, buscarías al usuario en la base de datos y verificarías la contraseña hasheada
        var usuario = await _usuarioRepository.GetByEmail(request.Email);
        if (usuario != null && VerifyPassword(request.Password, usuario.Password))
        {
            var token = GenerateJwtToken(usuario.Email);
            return new LoginResponse { Success = true, Message = "Inicio de sesión exitoso", Token = token };
        }
        return new LoginResponse { Success = false, Message = "Email o contraseña incorrectos" };
    }

    private string GenerateJwtToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("your_secret_key_here");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        // 1. Verificar si el email ya existe
        var existingUser = await _usuarioRepository.GetByEmail(request.Email);
        if (existingUser != null)
        {
            return false;
        }

        // 2. Hashear la contraseña
        var hashedPassword = HashPassword(request.Password);

        // 3. Crear el nuevo usuario usando el constructor por defecto
        var newUser = new Usuario
        {
            Nombre = request.Nombre,
            Email = request.Email,
            Dni = request.Dni,
            // Asignar la contraseña hasheada al campo del usuario
            // Por ejemplo: Password = hashedPassword;
        };

        // 4. Agregar el usuario a la base de datos
        var result = await _usuarioRepository.AddUsuario(newUser);
        return result > 0;
    }

    private string HashPassword(string password)
    {
        // En una aplicación real, usarías una librería de hashing segura como BCrypt o Argon2.
        // Este es solo un ejemplo simplificado para mostrar la estructura.
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        // En una aplicación real, usarías una librería de hashing segura como BCrypt o Argon2.
        // Este es solo un ejemplo simplificado.
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}