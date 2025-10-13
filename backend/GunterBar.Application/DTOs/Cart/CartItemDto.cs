namespace GunterBar.Application.DTOs.Cart;

public class CartItemDto
{
    public int Id { get; set; }
    public int DrinkId { get; set; }
    public string DrinkName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}
