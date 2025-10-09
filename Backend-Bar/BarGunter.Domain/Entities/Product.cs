namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }

    // Navigation property
    public Category? Category { get; set; }

    public Product() { }

    public Product(int productId, string name, string description, decimal price, int categoryId)
    {
        ProductId = productId;
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
    }
}
