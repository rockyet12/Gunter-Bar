namespace BarGunter.Domain.Entities;

public class Producto
{
    public int CDProducto { get; set; }
    public Carrito idCarrito { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }
     
}
