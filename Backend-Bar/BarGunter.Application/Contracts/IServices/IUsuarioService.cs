using BarGunter.Domain.Entities;
using BarGunter.Application.DTOs;

namespace BarGunter.Application.Contracts.IServices; 

public interface IUsuarioService
{
    Task<List<Usuario>> GetAllUsuarios();
    Task<Usuario> GetUsuarioById(int id);
    Task<int> AddUsuario(Usuario usuario);
    Task<bool> UpdateUsuario(Usuario usuario);
    Task<bool> DeleteUsuario(int id);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<bool> RegisterAsync(RegisterRequest request);
}