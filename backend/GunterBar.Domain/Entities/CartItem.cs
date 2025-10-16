using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunterBar.Domain.Common;

namespace GunterBar.Domain.Entities;

public class CartItem : EntityBase
{
    [Required, ForeignKey("Cart")]
    public int CartId { get; set; }

    [Required, ForeignKey("Drink")]
    public int DrinkId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    public DateTime AddedAt { get; set; }

    [NotMapped]
    public decimal Subtotal => Quantity * UnitPrice;

    public virtual Cart Cart { get; set; } = null!;
    public virtual Drink Drink { get; set; } = null!;

    protected CartItem() 
    {
        AddedAt = DateTime.UtcNow;
    }

    public CartItem(int cartId, int drinkId, int quantity, decimal unitPrice) : this()
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(quantity));
        
        if (unitPrice <= 0)
            throw new ArgumentException("El precio unitario debe ser mayor a 0", nameof(unitPrice));

        CartId = cartId;
        DrinkId = drinkId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(newQuantity));

        Quantity = newQuantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("El precio debe ser mayor a 0", nameof(newPrice));

        UnitPrice = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }
}