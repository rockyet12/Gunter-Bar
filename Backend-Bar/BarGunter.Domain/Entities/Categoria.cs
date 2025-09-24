namespace BarGunter.Domain.Entities;

public class Categoria
{
    public int IdCategoria { get; set; }
    public Producto CDProducto { get; set; }
    public Tipo IdTipo { get; set; }
    public string Descri { get; set; }
    public decimal Precio { get; set; }
    public Categoria(int Id, Producto cDProducto, Tipo idTipo, string descri, decimal precio)
    {
        IdCategoria = Id;
        CDProducto = cDProducto;
        IdTipo = idTipo;
        Descri = descri;
        Precio = precio;
    }
}
