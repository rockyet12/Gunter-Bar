namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Category() { }

    public Category(int categoryId, string name, string description)
    {
        CategoryId = categoryId;
        Name = name;
        Description = description;
    }
}
