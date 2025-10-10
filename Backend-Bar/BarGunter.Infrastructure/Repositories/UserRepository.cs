using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Domain.Entities;
using BarGunter.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace BarGunter.Infrastructure.Persistences.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BarGunterDbContext _context;

    public UserRepository(BarGunterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

    public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

    public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

    public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

    public async Task<User?> UpdateAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return null;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Gender = user.Gender;
            existingUser.Role = user.Role;
            existingUser.IsActive = user.IsActive;

            await _context.SaveChangesAsync();
            return existingUser;
        }

    public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

    public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
}

