using Microsoft.EntityFrameworkCore;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;
using GunterBar.Infrastructure.Data;

namespace GunterBar.Infrastructure.Repositories;

public class DrinkIngredientRepository : IDrinkIngredientRepository
{
    private readonly GunterBarDbContext _context;

    public DrinkIngredientRepository(GunterBarDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DrinkIngredient?> GetByIdAsync(int id)
    {
        return await _context.DrinkIngredients
            .Include(di => di.Drink)
            .AsNoTracking()
            .FirstOrDefaultAsync(di => di.Id == id);
    }

    public async Task<IEnumerable<DrinkIngredient>> GetByDrinkIdAsync(int drinkId)
    {
        return await _context.DrinkIngredients
            .Where(di => di.DrinkId == drinkId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<DrinkIngredient> CreateAsync(DrinkIngredient ingredient)
    {
        if (ingredient == null)
            throw new ArgumentNullException(nameof(ingredient));

        // Verificar que existe la bebida
        var drinkExists = await _context.Drinks.AnyAsync(d => d.Id == ingredient.DrinkId);
        if (!drinkExists)
            throw new KeyNotFoundException($"Bebida con ID {ingredient.DrinkId} no encontrada");

        // Verificar que no exista el mismo ingrediente para la misma bebida
        var existingIngredient = await _context.DrinkIngredients
            .FirstOrDefaultAsync(di => di.DrinkId == ingredient.DrinkId && 
                                     di.Name.ToLower() == ingredient.Name.ToLower());
        if (existingIngredient != null)
            throw new InvalidOperationException($"Ya existe el ingrediente {ingredient.Name} para esta bebida");

        await _context.DrinkIngredients.AddAsync(ingredient);
        await _context.SaveChangesAsync();
        return ingredient;
    }

    public async Task<DrinkIngredient> UpdateAsync(DrinkIngredient ingredient)
    {
        if (ingredient == null)
            throw new ArgumentNullException(nameof(ingredient));

        var existingIngredient = await _context.DrinkIngredients
            .FirstOrDefaultAsync(di => di.Id == ingredient.Id);

        if (existingIngredient == null)
            throw new KeyNotFoundException($"Ingrediente con ID {ingredient.Id} no encontrado");

        // Verificar que no exista el mismo nombre para otro ingrediente de la misma bebida
        if (existingIngredient.Name != ingredient.Name)
        {
            var nameExists = await _context.DrinkIngredients
                .AnyAsync(di => di.DrinkId == ingredient.DrinkId && 
                               di.Name.ToLower() == ingredient.Name.ToLower() && 
                               di.Id != ingredient.Id);
            if (nameExists)
                throw new InvalidOperationException($"Ya existe el ingrediente {ingredient.Name} para esta bebida");
        }

        _context.Entry(existingIngredient).CurrentValues.SetValues(ingredient);
        await _context.SaveChangesAsync();
        return existingIngredient;
    }

    public async Task DeleteAsync(int id)
    {
        var ingredient = await _context.DrinkIngredients.FindAsync(id);
        if (ingredient == null)
            throw new KeyNotFoundException($"Ingrediente con ID {id} no encontrado");

        _context.DrinkIngredients.Remove(ingredient);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.DrinkIngredients.AnyAsync(di => di.Id == id);
    }
}
