using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Domain.Entities;
using BarGunter.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Infrastructure.Persistences.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly BarGunterDbContext _context;

    public UsuarioRepository(BarGunterDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> GetAllUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario> GetUsuarioById(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }
    
    public async Task<Usuario> GetByEmail(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<int> AddUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario.Id;
    }

    public async Task<bool> UpdateUsuario(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return false;
        }
        _context.Usuarios.Remove(usuario);
        return await _context.SaveChangesAsync() > 0;
    }
}