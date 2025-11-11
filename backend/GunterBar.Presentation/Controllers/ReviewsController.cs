using System.Security.Claims;
using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Review;
using GunterBar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GunterBar.Presentation.Controllers;

/// <summary>
/// Controlador para gestionar reseñas
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly ILogger<ReviewsController> _logger;

    public ReviewsController(IReviewService reviewService, ILogger<ReviewsController> logger)
    {
        _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene todas las reseñas de un producto
    /// </summary>
    /// <param name="drinkId">ID del producto</param>
    /// <returns>Lista de reseñas</returns>
    [HttpGet("drink/{drinkId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ReviewDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ReviewDto>>>> GetByDrinkId(int drinkId)
    {
        try
        {
            var response = await _reviewService.GetReviewsByDrinkIdAsync(drinkId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reviews for drink {DrinkId}", drinkId);
            return StatusCode(500, new ApiResponse<IEnumerable<ReviewDto>>
            {
                Success = false,
                Message = "Error interno del servidor"
            });
        }
    }

    /// <summary>
    /// Obtiene las reseñas del usuario actual
    /// </summary>
    /// <returns>Lista de reseñas del usuario</returns>
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ReviewDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ReviewDto>>>> GetMyReviews()
    {
        try
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Forbid();

            var response = await _reviewService.GetReviewsByUserIdAsync(int.Parse(userId));
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user reviews");
            return StatusCode(500, new ApiResponse<IEnumerable<ReviewDto>>
            {
                Success = false,
                Message = "Error interno del servidor"
            });
        }
    }

    /// <summary>
    /// Crea una nueva reseña
    /// </summary>
    /// <param name="request">Datos de la reseña</param>
    /// <returns>Reseña creada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ReviewDto>>> Create(CreateReviewRequest request)
    {
        try
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Forbid();

            var response = await _reviewService.CreateReviewAsync(int.Parse(userId), request);
            if (response.Success)
                return CreatedAtAction(nameof(GetByDrinkId), new { drinkId = request.DrinkId }, response);
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating review");
            return StatusCode(500, new ApiResponse<ReviewDto>
            {
                Success = false,
                Message = "Error interno del servidor"
            });
        }
    }

    /// <summary>
    /// Actualiza una reseña existente
    /// </summary>
    /// <param name="id">ID de la reseña</param>
    /// <param name="request">Datos actualizados</param>
    /// <returns>Reseña actualizada</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ReviewDto>>> Update(int id, UpdateReviewRequest request)
    {
        try
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Forbid();

            var response = await _reviewService.UpdateReviewAsync(id, int.Parse(userId), request);
            if (response.Success)
                return Ok(response);
            return NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating review {ReviewId}", id);
            return StatusCode(500, new ApiResponse<ReviewDto>
            {
                Success = false,
                Message = "Error interno del servidor"
            });
        }
    }

    /// <summary>
    /// Elimina una reseña
    /// </summary>
    /// <param name="id">ID de la reseña</param>
    /// <returns>Resultado de la eliminación</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        try
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Forbid();

            var response = await _reviewService.DeleteReviewAsync(id, int.Parse(userId));
            if (response.Success)
                return Ok(response);
            return NotFound(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting review {ReviewId}", id);
            return StatusCode(500, new ApiResponse<bool>
            {
                Success = false,
                Message = "Error interno del servidor"
            });
        }
    }
}