using Microsoft.EntityFrameworkCore;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;
using GunterBar.Infrastructure.Data;

namespace GunterBar.Infrastructure.Repositories;

public class BarRepository : IBarRepository
{
    private readonly GunterBarDbContext _context;

    public BarRepository(GunterBarDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Bar?> GetByIdAsync(int id)
    {
        return await _context.Bars
            .Include(b => b.Owner)
            .Include(b => b.Drinks)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Bar?> GetByOwnerIdAsync(int ownerId)
    {
        return await _context.Bars
            .Include(b => b.Owner)
            .Include(b => b.Drinks)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.OwnerId == ownerId);
    }

    public async Task<IEnumerable<Bar>> GetAllAsync()
    {
        return await _context.Bars
            .Include(b => b.Owner)
            .Include(b => b.Drinks)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Bar>> GetActiveBarsAsync()
    {
        return await _context.Bars
            .Where(b => b.IsActive)
            .Include(b => b.Owner)
            .Include(b => b.Drinks)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Bar> CreateAsync(Bar bar)
    {
        if (bar == null)
            throw new ArgumentNullException(nameof(bar));

        if (string.IsNullOrWhiteSpace(bar.Name))
            throw new ArgumentException("El nombre del bar es requerido", nameof(bar.Name));

        if (bar.OwnerId <= 0)
            throw new ArgumentException("El ID del propietario es requerido", nameof(bar.OwnerId));

        // Verificar si ya existe un bar con el mismo nombre
        if (await BarExistsAsync(bar.Name))
            throw new InvalidOperationException($"Ya existe un bar con el nombre {bar.Name}");

        await _context.Bars.AddAsync(bar);
        await _context.SaveChangesAsync();

        return bar;
    }

    public async Task<Bar> UpdateAsync(Bar bar)
    {
        if (bar == null)
            throw new ArgumentNullException(nameof(bar));

        var existingBar = await _context.Bars
            .FindAsync(bar.Id);

        if (existingBar == null)
            throw new KeyNotFoundException($"Bar con ID {bar.Id} no encontrado");

        // Si el nombre cambió, verificar que el nuevo nombre no exista
        if (existingBar.Name != bar.Name && await BarExistsAsync(bar.Name, bar.Id))
            throw new InvalidOperationException($"Ya existe un bar con el nombre {bar.Name}");

        // Actualizar campos
        existingBar.Name = bar.Name;
        existingBar.Description = bar.Description;
        existingBar.Address = bar.Address;
        existingBar.City = bar.City;
        existingBar.PostalCode = bar.PostalCode;
        existingBar.Country = bar.Country;
        existingBar.Latitude = bar.Latitude;
        existingBar.Longitude = bar.Longitude;
        existingBar.PhoneNumber = bar.PhoneNumber;
        existingBar.Email = bar.Email;
        existingBar.ImageUrl = bar.ImageUrl;
        existingBar.OpeningHours = bar.OpeningHours;
        existingBar.IsActive = bar.IsActive;
        existingBar.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingBar;
    }

    public async Task DeleteAsync(int id)
    {
        var bar = await _context.Bars.FindAsync(id);
        if (bar == null)
            throw new KeyNotFoundException($"Bar con ID {id} no encontrado");

        _context.Bars.Remove(bar);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> BarExistsAsync(string name, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del bar no puede estar vacío", nameof(name));

        var query = _context.Bars.Where(b => b.Name.ToLower() == name.ToLower());

        if (excludeId.HasValue)
        {
            query = query.Where(b => b.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}
