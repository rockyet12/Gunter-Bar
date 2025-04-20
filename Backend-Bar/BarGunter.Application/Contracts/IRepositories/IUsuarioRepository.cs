
using BarGunter.Domain.Entities;

public interface IUsuarioRepository 
{
    Task<Usuario> GetByDni(int dni);
    Task<Usuario> GetByNickName(string nickName);
    Task<Usuario> GetByEmail(string email);
    Task<Usuario> GetById(int id);
    Task<bool> ExistDni(int dni);
    Task<bool> ExistNickName(string nickName);
    Task<bool> ExistEmail(string email);
}