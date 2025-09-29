using BarGunter.Domain.Entities;
using BarGunter.Domain.Enums;

namespace BarGunter.Application.Contracts.IServices;

public interface IpedidoService
{
    Task<List<Pedido>> GetAllPedidos();
    Task<Pedido> GetPedidoById(int IdPedido);
    Task<int> AddPedido(Pedido pedido);

    Task<bool> UpdateEstado(int IdPedido, EstadoPedido nuevoEstado);
}
