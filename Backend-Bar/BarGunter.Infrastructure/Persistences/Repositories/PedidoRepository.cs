using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Domain.Entities;
using BarGunter.Domain.Enums;
using BarGunter.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Infrastructure.Persistences.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly BarGunterDbContext _context;

    public PedidoRepository(BarGunterDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pedido>> GetAllPedidos()
    {
        return await _context.Pedidos.ToListAsync();
    }

    public async Task<Pedido> GetPedidoById(int id)
    {
        return await _context.Pedidos.FindAsync(id);
    }

    public async Task<int> AddPedido(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        return pedido.IdPedido;
    }

    public async Task<bool> UpdateEstado(int idPedido, EstadoPedido nuevoEstado)
    {
        var pedido = await _context.Pedidos.FindAsync(idPedido);
        if (pedido == null)
        {
            return false;
        }
        
        // Simplemente actualiza el estado si es un cambio válido
        pedido.Estado = nuevoEstado;
        
        return await _context.SaveChangesAsync() > 0;
    }
}