using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using GunterBar.Infrastructure.Data;

namespace GunterBar.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly GunterBarDbContext _context;

    public ReviewRepository(GunterBarDbContext context)
    {
        _context = context;
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task<Review> AddAsync(Review entity)
    {
        _context.Reviews.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Review entity)
    {
        _context.Reviews.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var review = await GetByIdAsync(id);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Reviews.AnyAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Review>> GetReviewsByDrinkIdAsync(int drinkId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Drink)
            .Where(r => r.DrinkId == drinkId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Drink)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review?> GetByUserAndDrinkAsync(int userId, int drinkId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.DrinkId == drinkId);
    }

    public async Task<Review?> GetByIdAndUserAsync(int reviewId, int userId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);
    }

    public async Task LoadUserAndDrinkAsync(Review review)
    {
        await _context.Entry(review).Reference(r => r.User).LoadAsync();
        await _context.Entry(review).Reference(r => r.Drink).LoadAsync();
    }
}