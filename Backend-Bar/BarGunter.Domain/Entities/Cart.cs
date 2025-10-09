namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Cart
{
    [Key]
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    // Navigation property
    public Product? Product { get; set; }

    public Cart() { }

    public Cart(int cartId, int productId, int quantity, decimal price)
    {
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
}
