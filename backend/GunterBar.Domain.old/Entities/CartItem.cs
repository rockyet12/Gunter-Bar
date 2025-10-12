namespace GunterBar.Domain.Entities;

// Entidad que representa un item dentro del carrito de compras
// Relaciona una bebida con su cantidad seleccionada

public class CartItem
{
    // Identificador único del item del carrito
    public int Id { get; set; }

    // ID del carrito al que pertenece este item
    public int CartId { get; set; }

    // Carrito al que pertenece este item
    public virtual Cart Cart { get; set; } = null!;

    // ID de la bebida seleccionada
    public int DrinkId { get; set; }

    // Bebida seleccionada
    public virtual Drink Drink { get; set; } = null!;

    // Cantidad de la bebida en el carrito
    public int Quantity { get; set; }

    // Precio unitario al momento de agregar al carrito
    public decimal UnitPrice { get; set; }

    // Fecha en que se agregó el item al carrito
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}