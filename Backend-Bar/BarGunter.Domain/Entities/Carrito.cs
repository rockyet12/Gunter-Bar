namespace BarGunter.Domain.Entities;

public class Carrito
{
    public int IdCarrito { get; set; }
    public Usuario IdUsuario { get; set; }
    public Producto CDProducto { get; set; }

    public Carrito(int Id, Usuario idUsuario, Producto cDProducto)
    {
        IdCarrito = Id;
        IdUsuario = idUsuario;
        CDProducto = cDProducto;
    } 
}
