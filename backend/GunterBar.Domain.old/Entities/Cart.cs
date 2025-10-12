using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunterBar.Domain.Entities;

namespace GunterBar.Domain.Entities;

public class Cart
{
    [Key]
    public int Id { get; set; }

    [Required, ForeignKey("User")]
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    // Constructor
    public Cart(int userId)
    {
        UserId = userId;
    }
}