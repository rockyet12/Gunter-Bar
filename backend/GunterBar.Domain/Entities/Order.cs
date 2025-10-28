using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunterBar.Application.ValueObjects;
using GunterBar.Domain.Common;
using GunterBar.Domain.Enums;
using GunterBar.Domain.ValueObjects;

namespace GunterBar.Domain.Entities;

public class Order : EntityBase
{
    private const int MaxItemsPerOrder = 20;
    private static readonly Dictionary<OrderStatus, OrderStatus[]> AllowedStatusTransitions = new()
    {
        { OrderStatus.Pending, new[] { OrderStatus.InProgress, OrderStatus.Cancelled } },
        { OrderStatus.InProgress, new[] { OrderStatus.Completed, OrderStatus.Cancelled } },
        { OrderStatus.Completed, Array.Empty<OrderStatus>() },
        { OrderStatus.Cancelled, Array.Empty<OrderStatus>() }
    };

    [Required, ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    public OrderStatus Status { get; private set; }


    [MaxLength(500)]
    public string? Notes { get; private set; }

    [Required]
    [MaxLength(200)]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string MetodoPago { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Tarjeta { get; set; }

    [Required]
    [MaxLength(4)]
    public string CodigoVerif { get; set; } = string.Empty;

    [Required]
    public DateTime OrderDate { get; private set; }

    public DateTime? CompletedDate { get; private set; }
    public DateTime? CancelledDate { get; private set; }

    [NotMapped]
    public Money Total => new Money(Items.Sum(i => i.Subtotal));

    [NotMapped]
    public int TotalItems => Items.Sum(i => i.Quantity);

    [NotMapped]
    public bool CanBeCancelled => Status is OrderStatus.Pending or OrderStatus.InProgress;

    [NotMapped]
    public bool CanBeModified => Status == OrderStatus.Pending;

    public virtual User User { get; set; } = null!;
    public virtual ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

    protected Order() 
    {
        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Pending;
    }

    public Order(int userId, string? notes = null, string direccion = "", string metodoPago = "", string? tarjeta = null, string codigoVerif = "") : this()
    {
        if (userId <= 0)
            throw new ArgumentException("El ID del usuario debe ser mayor a 0", nameof(userId));

        if (notes?.Length > 500)
            throw new ArgumentException("Las notas no pueden exceder los 500 caracteres", nameof(notes));

        if (string.IsNullOrWhiteSpace(direccion))
            throw new ArgumentException("La dirección es obligatoria", nameof(direccion));

        if (string.IsNullOrWhiteSpace(metodoPago))
            throw new ArgumentException("El método de pago es obligatorio", nameof(metodoPago));

        if (string.IsNullOrWhiteSpace(codigoVerif) || codigoVerif.Length != 4)
            throw new ArgumentException("El código de verificación debe tener 4 caracteres", nameof(codigoVerif));

        UserId = userId;
        Notes = notes?.Trim();
        Direccion = direccion.Trim();
        MetodoPago = metodoPago.Trim();
        Tarjeta = tarjeta?.Trim();
        CodigoVerif = codigoVerif.Trim();
    }

    public void AddItem(OrderItem item)
    {
        if (!CanBeModified)
            throw new InvalidOperationException($"No se pueden agregar items a una orden en estado {Status}");

        if (Items.Count >= MaxItemsPerOrder)
            throw new InvalidOperationException($"No se pueden agregar más de {MaxItemsPerOrder} items a una orden");

        var existingItem = Items.FirstOrDefault(i => i.DrinkId == item.DrinkId);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
            UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            Items.Add(item);
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(int itemId)
    {
        if (!CanBeModified)
            throw new InvalidOperationException($"No se pueden eliminar items de una orden en estado {Status}");

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new InvalidOperationException("Item no encontrado en la orden");

        Items.Remove(item);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(OrderStatus newStatus, string? notes = null)
    {
        if (!CanTransitionTo(newStatus))
            throw new InvalidOperationException($"No se puede cambiar el estado de {Status} a {newStatus}");

        if (notes?.Length > 500)
            throw new ArgumentException("Las notas no pueden exceder los 500 caracteres", nameof(notes));

        Status = newStatus;
        Notes = notes?.Trim() ?? Notes;

        switch (newStatus)
        {
            case OrderStatus.Completed:
                CompletedDate = DateTime.UtcNow;
                break;
            case OrderStatus.Cancelled:
                CancelledDate = DateTime.UtcNow;
                break;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateNotes(string? notes)
    {
        if (notes?.Length > 500)
            throw new ArgumentException("Las notas no pueden exceder los 500 caracteres", nameof(notes));

        Notes = notes?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    private bool CanTransitionTo(OrderStatus newStatus)
    {
        return AllowedStatusTransitions.ContainsKey(Status) && 
               AllowedStatusTransitions[Status].Contains(newStatus);
    }
}