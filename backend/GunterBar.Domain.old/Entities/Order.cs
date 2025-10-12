using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GunterBar.Domain.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required, ForeignKey("User")]
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    // Constructor
    public Order(int userId, decimal totalAmount)
    {
        UserId = userId;
        TotalAmount = totalAmount;
    }
}