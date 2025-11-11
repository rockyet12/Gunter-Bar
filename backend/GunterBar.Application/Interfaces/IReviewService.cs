using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Review;

namespace GunterBar.Application.Interfaces;

public interface IReviewService
{
    Task<ApiResponse<IEnumerable<ReviewDto>>> GetReviewsByDrinkIdAsync(int drinkId);
    Task<ApiResponse<IEnumerable<ReviewDto>>> GetReviewsByUserIdAsync(int userId);
    Task<ApiResponse<ReviewDto>> CreateReviewAsync(int userId, CreateReviewRequest request);
    Task<ApiResponse<ReviewDto>> UpdateReviewAsync(int reviewId, int userId, UpdateReviewRequest request);
    Task<ApiResponse<bool>> DeleteReviewAsync(int reviewId, int userId);
}