using System;

namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Ticket
{
    [Key]
    public int IdTicket { get; set; }
    public int IdUsuario { get; set; }
    public int IdPedido { get; set; } // Enlace a la orden de logística (Pedido)
    public DateTime FechaCompra { get; set; }
    public decimal Total { get; set; }

    public Ticket()
    {
        FechaCompra = DateTime.Now;
        Total = 0;
    }
}