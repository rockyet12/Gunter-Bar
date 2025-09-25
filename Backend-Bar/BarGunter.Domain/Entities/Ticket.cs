using System.Collections;

namespace BarGunter.Domain.Entities;

public class Ticket
{
    public int IdTicket { get; set; }
    public Usuario IdUsuario { get; set; }
    public DateTime Fecha { get; set; }
    public Producto precio { get; set; }
    public decimal Total { get; set; }
    public int Cantidad { get; set; }
    public Ticket (int Id, Usuario Idusuario, DateTime Fecha, Producto precio, decimal total, int cantidad)
    {
        IdTicket = Id;
        IdUsuario = Idusuario;
        this.Fecha = Fecha;
        this.precio = precio;
        Total = total;
        Cantidad = cantidad;
    }
}
