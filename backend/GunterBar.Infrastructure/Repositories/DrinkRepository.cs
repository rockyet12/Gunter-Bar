using Microsoft.EntityFrameworkCore;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;
using GunterBar.Domain.Enums;
using GunterBar.Infrastructure.Data;

namespace GunterBar.Infrastructure.Repositories;

public class DrinkRepository : IDrinkRepository
{
    private readonly GunterBarDbContext _context;

    public DrinkRepository(GunterBarDbContext context)
    {
        _context = context;
    }

    public async Task<Drink> GetByIdAsync(int id)
    {
        var drink = await _context.Drinks
            .Include(d => d.Ingredients)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (drink == null)
            throw new KeyNotFoundException($"Bebida con ID {id} no encontrada.");

        return drink;
    }

    public async Task<IEnumerable<Drink>> GetAllAsync()
    {
        return await _context.Drinks
            .Include(d => d.Ingredients)
            .ToListAsync();
    }

    public async Task<IEnumerable<Drink>> GetByTypeAsync(DrinkType type)
    {
        // Como Type no existe en la entidad Drink, retornamos todas las bebidas
        // TODO: Implementar la lógica de filtrado por tipo cuando se agregue la propiedad Type
        return await GetAllAsync();
    }

    public async Task<IEnumerable<Drink>> GetAvailableAsync()
    {
        return await _context.Drinks
            .Include(d => d.Ingredients)
            .Where(d => d.IsAvailable)
            .ToListAsync();
    }

    public async Task<Drink> CreateAsync(Drink drink)
    {
        await _context.Drinks.AddAsync(drink);
        await _context.SaveChangesAsync();
        return drink;
    }

    public async Task<Drink> UpdateAsync(Drink drink)
    {
        var existingDrink = await _context.Drinks
            .Include(d => d.Ingredients)
            .FirstOrDefaultAsync(d => d.Id == drink.Id);

        if (existingDrink == null)
            throw new KeyNotFoundException($"Bebida con ID {drink.Id} no encontrada.");

        // Actualizar propiedades básicas
        _context.Entry(existingDrink).CurrentValues.SetValues(drink);
        
        // Actualizar ingredientes
        foreach (var ingredient in drink.Ingredients)
        {
            var existingIngredient = existingDrink.Ingredients
                .FirstOrDefault(i => i.Id == ingredient.Id);

            if (existingIngredient == null)
            {
                existingDrink.Ingredients.Add(ingredient);
            }
            else
            {
                _context.Entry(existingIngredient).CurrentValues.SetValues(ingredient);
            }
        }

        // Eliminar ingredientes que ya no existen
        foreach (var existingIngredient in existingDrink.Ingredients.ToList())
        {
            if (!drink.Ingredients.Any(i => i.Id == existingIngredient.Id))
            {
                existingDrink.Ingredients.Remove(existingIngredient);
            }
        }

        await _context.SaveChangesAsync();
        return existingDrink;
    }

    public async Task DeleteAsync(int id)
    {
        var drink = await _context.Drinks.FindAsync(id);
        if (drink == null)
            throw new KeyNotFoundException($"Bebida con ID {id} no encontrada.");

        _context.Drinks.Remove(drink);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateStockAsync(int drinkId, int newStock)
    {
        var drink = await _context.Drinks.FindAsync(drinkId);
        if (drink == null)
            return false;

        try
        {
            drink.UpdateStock(newStock);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
