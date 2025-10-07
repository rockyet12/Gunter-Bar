using BarGunter.Domain.Enums;

namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Pedido
{
    [Key]
    public int IdPedido { get; set; }
    public int IdUsuario { get; set; }
    public DateTime FechaPedido { get; set; }
    public decimal Total { get; set; }
    public EstadoPedido Estado { get; set; }
    public string DireccionEntrega { get; set; }

    public Pedido()
    {
        FechaPedido = DateTime.Now;
        Estado = EstadoPedido.Pendiente;
        DireccionEntrega = string.Empty;
    }
}
