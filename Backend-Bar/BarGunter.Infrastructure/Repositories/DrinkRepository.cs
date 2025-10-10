using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Domain.Entities;
using BarGunter.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace BarGunter.Infrastructure.Persistences.Repositories;
    public class DrinkRepository : IDrinkRepository
    {
        private readonly BarGunterDbContext _context;

        public DrinkRepository(BarGunterDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Drink>> GetAllAsync()
        {
            return await _context.Drinks.ToListAsync();
        }

        public async Task<Drink?> GetByIdAsync(int id)
        {
            return await _context.Drinks.FindAsync(id);
        }

        public async Task<Drink> CreateAsync(Drink drink)
        {
            _context.Drinks.Add(drink);
            await _context.SaveChangesAsync();
            return drink;
        }

        public async Task<Drink?> UpdateAsync(int id, Drink drink)
        {
            var existingDrink = await _context.Drinks.FindAsync(id);
            if (existingDrink == null) return null;

            existingDrink.Name = drink.Name;
            existingDrink.Price = drink.Price;
            existingDrink.Description = drink.Description;
            existingDrink.TypeId = drink.TypeId;

            await _context.SaveChangesAsync();
            return existingDrink;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var drink = await _context.Drinks.FindAsync(id);
            if (drink == null) return false;

            _context.Drinks.Remove(drink);
            await _context.SaveChangesAsync();
            return true;
        }
    }

