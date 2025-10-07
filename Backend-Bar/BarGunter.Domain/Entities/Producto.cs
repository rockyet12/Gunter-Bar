namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Producto
{
    [Key]
    public int CDProducto { get; set; }
    public Carrito? idCarrito { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }

    public Producto() { }

    public Producto(int CDProducto, Carrito idcarrito, string Nombre, string Descripcion, decimal Precio)
    {
        this.CDProducto = CDProducto;
        this.idCarrito = idcarrito;
        this.Nombre = Nombre;
        this.Descripcion = Descripcion;
        this.Precio = Precio;
    }
}
