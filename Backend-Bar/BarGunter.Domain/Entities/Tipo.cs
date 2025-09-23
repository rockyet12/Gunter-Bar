namespace BarGunter.Domain.Entities;

public class Tipo
{
    public int IdTipo { get; set; }
    public string Nombre { get; set; }

    public Tipo(int idTipo, string nombre)
    {
        IdTipo = idTipo;
        Nombre = nombre;
    }
}
