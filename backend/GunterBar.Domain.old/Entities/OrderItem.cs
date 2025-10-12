using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GunterBar.Domain.Entities;

public class OrderItem
{
    [Key]
    public int Id { get; set; }

    [Required, ForeignKey("Order")]
    public int OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;

    [Required, ForeignKey("Drink")]
    public int DrinkId { get; set; }

    public virtual Drink Drink { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    // Constructor
    public OrderItem(int orderId, int drinkId, int quantity, decimal price)
    {
        OrderId = orderId;
        DrinkId = drinkId;
        Quantity = quantity;
        Price = price;
    }
}