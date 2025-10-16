using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunterBar.Domain.Common;

namespace GunterBar.Domain.Entities;

public class OrderItem : EntityBase
{
    [Required, ForeignKey("Order")]
    public int OrderId { get; set; }

    [Required, ForeignKey("Drink")]
    public int DrinkId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [MaxLength(100)]
    public string DrinkName { get; set; } = string.Empty;

    [NotMapped]
    public decimal Subtotal => Quantity * UnitPrice;

    public virtual Order Order { get; set; } = null!;
    public virtual Drink Drink { get; set; } = null!;

    protected OrderItem() { }

    public OrderItem(int orderId, int drinkId, string drinkName, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(quantity));
        
        if (unitPrice <= 0)
            throw new ArgumentException("El precio unitario debe ser mayor a 0", nameof(unitPrice));

        if (string.IsNullOrWhiteSpace(drinkName))
            throw new ArgumentException("El nombre de la bebida es requerido", nameof(drinkName));

        OrderId = orderId;
        DrinkId = drinkId;
        DrinkName = drinkName.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}