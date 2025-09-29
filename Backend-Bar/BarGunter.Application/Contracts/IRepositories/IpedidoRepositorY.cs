using BarGunter.Domain.Entities;
using BarGunter.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Contracts.IRepositories;

public interface IPedidoRepository
{
    Task<List<Pedido>> GetAllPedidos();
    Task<Pedido> GetPedidoById(int id);
    Task<int> AddPedido(Pedido pedido);
    Task<bool> UpdateEstado(int idPedido, EstadoPedido nuevoEstado);
}