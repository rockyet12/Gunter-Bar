using GunterBar.Domain.Entities;

namespace GunterBar.Domain.Interfaces;

public interface IReviewRepository : IRepository<Review>
{
    Task<IEnumerable<Review>> GetReviewsByDrinkIdAsync(int drinkId);
    Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);
    Task<Review?> GetByUserAndDrinkAsync(int userId, int drinkId);
    Task<Review?> GetByIdAndUserAsync(int reviewId, int userId);
    Task LoadUserAndDrinkAsync(Review review);
}