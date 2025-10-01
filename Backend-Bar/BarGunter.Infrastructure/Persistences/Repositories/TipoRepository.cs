using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Domain.Entities;
using BarGunter.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Infrastructure.Persistences.Repositories;

public class TipoRepository : ITipoRepository
{
    private readonly BarGunterDbContext _context;

    public TipoRepository(BarGunterDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tipo>> GetAllTipos()
    {
        return await _context.Tipos.ToListAsync();
    }

    public async Task<Tipo> GetTipoById(int id)
    {
        return await _context.Tipos.FindAsync(id);
    }

    public async Task<int> AddTipo(Tipo tipo)
    {
        _context.Tipos.Add(tipo);
        await _context.SaveChangesAsync();
        return tipo.IdTipo;
    }

    public async Task<bool> UpdateTipo(Tipo tipo)
    {
        _context.Tipos.Update(tipo);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteTipo(int id)
    {
        var tipo = await _context.Tipos.FindAsync(id);
        if (tipo == null)
        {
            return false;
        }
        _context.Tipos.Remove(tipo);
        return await _context.SaveChangesAsync() > 0;
    }
}