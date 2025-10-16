using Microsoft.EntityFrameworkCore;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;
using GunterBar.Infrastructure.Data;

namespace GunterBar.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GunterBarDbContext _context;

    public UserRepository(GunterBarDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Orders)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío", nameof(email));

        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Orders)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("El email es requerido", nameof(user.Email));

        if (string.IsNullOrWhiteSpace(user.PasswordHash))
            throw new ArgumentException("El hash de la contraseña es requerido", nameof(user.PasswordHash));

        // Verificar si el email ya existe
        if (await EmailExistsAsync(user.Email))
            throw new InvalidOperationException($"Ya existe un usuario con el email {user.Email}");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var existingUser = await _context.Users
            .FindAsync(user.Id);

        if (existingUser == null)
            throw new KeyNotFoundException($"Usuario con ID {user.Id} no encontrado");

        // Si el email cambió, verificar que el nuevo email no exista
        if (existingUser.Email != user.Email && await EmailExistsAsync(user.Email))
            throw new InvalidOperationException($"Ya existe un usuario con el email {user.Email}");

        // Actualizar solo los campos permitidos
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.Address = user.Address;
        existingUser.Role = user.Role;
        
        // No actualizar el PasswordHash aquí - debe hacerse a través de un método específico

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"Usuario con ID {id} no encontrado");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío", nameof(email));

        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    // Métodos adicionales para autenticación
    public async Task UpdateLoginAttemptsAsync(User user, bool resetAttempts = false)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (resetAttempts)
        {
            user.LoginAttempts = 0;
            user.LastLoginAttempt = null;
        }
        else
        {
            user.LoginAttempts++;
            user.LastLoginAttempt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdatePasswordHashAsync(int userId, string newPasswordHash)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado");

        user.PasswordHash = newPasswordHash;
        await _context.SaveChangesAsync();
    }
}
