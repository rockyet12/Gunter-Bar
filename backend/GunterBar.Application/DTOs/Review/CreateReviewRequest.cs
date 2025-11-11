using System.ComponentModel.DataAnnotations;

namespace GunterBar.Application.DTOs.Review;

public class CreateReviewRequest
{
    [Required]
    public int DrinkId { get; set; }

    [Required, Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; }
}