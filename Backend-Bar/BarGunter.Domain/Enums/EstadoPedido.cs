namespace BarGunter.Domain.Enums;

public enum EstadoPedido :byte
{
    Pendiente = 1,
    EnPreparacion = 2,
    EnCamino = 3,
    Entregado = 4,
    Cancelado =5
}
