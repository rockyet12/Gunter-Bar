namespace BarGunter.Domain.Entities;

public class Tragos
{
    public int IdTragos { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string Ingredientes { get; set; }
    public int CDProducto { get; set; }
    public int IdTipo { get; set; }
    public Producto? producto { get; set; }
    public Tipo? tipo { get; set; }

    public Tragos() { }

    public Tragos(int IdTragos, string Nombre, string Descripcion, string Ingredientes, int CDProducto, int IdTipo, Producto producto, Tipo tipo)
    {
        this.IdTragos = IdTragos;
        this.Nombre = Nombre;
        this.Descripcion = Descripcion;
        this.Ingredientes = Ingredientes;
        this.CDProducto = CDProducto;
        this.IdTipo = IdTipo;
        this.producto = producto;
        this.tipo = tipo;
    }
}
