using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using BarGunter.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Services;

public class PedidoService : IpedidoService
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoService(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<List<Pedido>> GetAllPedidos()
    {
        return await _pedidoRepository.GetAllPedidos();
    }

    public async Task<Pedido> GetPedidoById(int id)
    {
        return await _pedidoRepository.GetPedidoById(id);
    }

    public async Task<int> AddPedido(Pedido pedido)
    {
        return await _pedidoRepository.AddPedido(pedido);
    }

    public async Task<bool> UpdateEstado(int idPedido, EstadoPedido nuevoEstado)
    {
        // Puedes agregar lógica de negocio aquí, por ejemplo:
        // if (nuevoEstado == EstadoPedido.Entregado && pedido.Total < 10) return false;
        
        return await _pedidoRepository.UpdateEstado(idPedido, nuevoEstado);
    }
}