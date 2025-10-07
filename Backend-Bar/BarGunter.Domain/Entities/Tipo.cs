namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Tipo
{
    [Key]
    public int IdTipo { get; set; }
    public string Nombre { get; set; }

    public Tipo()
    {
        Nombre = string.Empty;
    }
}
