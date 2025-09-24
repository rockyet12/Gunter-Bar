namespace BarGunter.Domain.Entities;

public class Tragos
{
    public int IdTragos { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string Ingredientes { get; set; }
    public int CDproducto { get; set; }
    public int IdTipo { get; set; }
    public Producto producto { get; set; }
    public Tipo tipo { get; set; }
}
