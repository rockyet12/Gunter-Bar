using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Domain.Entities;
using BarGunter.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Infrastructure.Persistences.Repositories;

public class TragoRepository : ITragoRepository
{
    private readonly BarGunterDbContext _context;

    public TragoRepository(BarGunterDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tragos>> GetAllTragos()
    {
        return await _context.Tragos.ToListAsync();
    }

    public async Task<Tragos> GetTragoById(int id)
    {
        return await _context.Tragos.FindAsync(id);
    }

    public async Task<int> AddTrago(Tragos trago)
    {
        _context.Tragos.Add(trago);
        await _context.SaveChangesAsync();
        return trago.IdTragos;
    }

    public async Task<bool> UpdateTrago(Tragos trago)
    {
        _context.Tragos.Update(trago);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteTrago(int id)
    {
        var trago = await _context.Tragos.FindAsync(id);
        if (trago == null)
        {
            return false;
        }
        _context.Tragos.Remove(trago);
        return await _context.SaveChangesAsync() > 0;
    }
}