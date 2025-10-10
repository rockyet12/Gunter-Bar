using BarGunter.Application.Interfaces.IRepositories;
using BarGunter.Domain.Entities;
using BarGunter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BarGunter.Infrastructure.Repositories;
    public class DrinkTypeRepository : IDrinkTypeRepository
    {
        private readonly BarGunterDbContext _context;

        public DrinkTypeRepository(BarGunterDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DrinkType>> GetAllAsync()
        {
            return await _context.DrinkTypes.ToListAsync();
        }

        public async Task<DrinkType?> GetByIdAsync(int id)
        {
            return await _context.DrinkTypes.FindAsync(id);
        }

        public async Task<DrinkType> CreateAsync(DrinkType drinkType)
        {
            _context.DrinkTypes.Add(drinkType);
            await _context.SaveChangesAsync();
            return drinkType;
        }

        public async Task<DrinkType?> UpdateAsync(int id, DrinkType drinkType)
        {
            var existingDrinkType = await _context.DrinkTypes.FindAsync(id);
            if (existingDrinkType == null) return null;

            existingDrinkType.Name = drinkType.Name;
            existingDrinkType.Description = drinkType.Description;

            await _context.SaveChangesAsync();
            return existingDrinkType;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var drinkType = await _context.DrinkTypes.FindAsync(id);
            if (drinkType == null) return false;

            _context.DrinkTypes.Remove(drinkType);
            await _context.SaveChangesAsync();
            return true;
        }
    }

