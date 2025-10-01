using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Domain.Entities;
using BarGunter.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Infrastructure.Persistences.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly BarGunterDbContext _context;

    public CategoriaRepository(BarGunterDbContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> GetAllCategorias()
    {
        return await _context.Categorias.ToListAsync();
    }

    public async Task<Categoria> GetCategoriaById(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }

    public async Task<int> AddCategoria(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria.IdCategoria;
    }

    public async Task<bool> UpdateCategoria(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteCategoria(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null)
        {
            return false;
        }
        _context.Categorias.Remove(categoria);
        return await _context.SaveChangesAsync() > 0;
    }
}