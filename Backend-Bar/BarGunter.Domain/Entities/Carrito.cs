namespace BarGunter.Domain.Entities;

public class Carrito
{
    public int IdCarrito { get; set; }
    // Navigation properties can be nullable to allow EF to construct the entity at design time
    public Usuario? IdUsuario { get; set; }
    public Producto? CDProducto { get; set; }

    // Parameterless constructor required by EF for materialization
    public Carrito() { }

    // Convenience constructor
    public Carrito(int Id, Usuario idUsuario, Producto cDProducto)
    {
        IdCarrito = Id;
        IdUsuario = idUsuario;
        CDProducto = cDProducto;
    }
}
