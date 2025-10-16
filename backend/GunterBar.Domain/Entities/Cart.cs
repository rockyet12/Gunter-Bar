using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunterBar.Domain.Common;

namespace GunterBar.Domain.Entities;

public class Cart : EntityBase
{
    [Required, ForeignKey("User")]
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    [NotMapped]
    public decimal Total => Items.Sum(i => i.Subtotal);

    [NotMapped]
    public int TotalItems => Items.Sum(i => i.Quantity);

    protected Cart() { }

    public Cart(int userId)
    {
        UserId = userId;
    }

    // Methods
    public void AddItem(CartItem item)
    {
        var existingItem = Items.FirstOrDefault(i => i.DrinkId == item.DrinkId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + item.Quantity);
        }
        else
        {
            Items.Add(item);
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(int itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            Items.Remove(item);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void Clear()
    {
        Items.Clear();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateItemQuantity(int itemId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            item.UpdateQuantity(quantity);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}