using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Review;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDrinkRepository _drinkRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReviewService(IReviewRepository reviewRepository, IUserRepository userRepository, IDrinkRepository drinkRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _drinkRepository = drinkRepository ?? throw new ArgumentNullException(nameof(drinkRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ApiResponse<IEnumerable<ReviewDto>>> GetReviewsByDrinkIdAsync(int drinkId)
    {
        try
        {
            var reviews = await _reviewRepository.GetReviewsByDrinkIdAsync(drinkId);
            var reviewDtos = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                UserId = r.UserId,
                UserName = r.User.Name,
                DrinkId = r.DrinkId,
                DrinkName = r.Drink.Name,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });

            return new ApiResponse<IEnumerable<ReviewDto>>
            {
                Success = true,
                Data = reviewDtos
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<ReviewDto>>
            {
                Success = false,
                Message = $"Error al obtener reseñas: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<ReviewDto>>> GetReviewsByUserIdAsync(int userId)
    {
        try
        {
            var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);
            var reviewDtos = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                UserId = r.UserId,
                UserName = r.User.Name,
                DrinkId = r.DrinkId,
                DrinkName = r.Drink.Name,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });

            return new ApiResponse<IEnumerable<ReviewDto>>
            {
                Success = true,
                Data = reviewDtos
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<ReviewDto>>
            {
                Success = false,
                Message = $"Error al obtener reseñas del usuario: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<ReviewDto>> CreateReviewAsync(int userId, CreateReviewRequest request)
    {
        try
        {
            // Check if user already reviewed this drink
            var existingReview = await _reviewRepository.GetByUserAndDrinkAsync(userId, request.DrinkId);
            if (existingReview != null)
            {
                return new ApiResponse<ReviewDto>
                {
                    Success = false,
                    Message = "Ya has reseñado este producto"
                };
            }

            var review = new Review
            {
                UserId = userId,
                DrinkId = request.DrinkId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            // Load related data
            await _reviewRepository.LoadUserAndDrinkAsync(review);

            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                UserId = review.UserId,
                UserName = review.User.Name,
                DrinkId = review.DrinkId,
                DrinkName = review.Drink.Name,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            };

            return new ApiResponse<ReviewDto>
            {
                Success = true,
                Data = reviewDto,
                Message = "Reseña creada exitosamente"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ReviewDto>
            {
                Success = false,
                Message = $"Error al crear reseña: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<ReviewDto>> UpdateReviewAsync(int reviewId, int userId, UpdateReviewRequest request)
    {
        try
        {
            var review = await _reviewRepository.GetByIdAndUserAsync(reviewId, userId);
            if (review == null)
            {
                return new ApiResponse<ReviewDto>
                {
                    Success = false,
                    Message = "Reseña no encontrada"
                };
            }

            review.Rating = request.Rating;
            review.Comment = request.Comment;
            review.UpdatedAt = DateTime.UtcNow;

            _reviewRepository.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync();

            // Load related data
            await _reviewRepository.LoadUserAndDrinkAsync(review);

            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                UserId = review.UserId,
                UserName = review.User.Name,
                DrinkId = review.DrinkId,
                DrinkName = review.Drink.Name,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            };

            return new ApiResponse<ReviewDto>
            {
                Success = true,
                Data = reviewDto,
                Message = "Reseña actualizada exitosamente"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ReviewDto>
            {
                Success = false,
                Message = $"Error al actualizar reseña: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<bool>> DeleteReviewAsync(int reviewId, int userId)
    {
        try
        {
            var review = await _reviewRepository.GetByIdAndUserAsync(reviewId, userId);
            if (review == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Reseña no encontrada"
                };
            }

            await _reviewRepository.DeleteAsync(review.Id);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = "Reseña eliminada exitosamente"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = $"Error al eliminar reseña: {ex.Message}"
            };
        }
    }
}