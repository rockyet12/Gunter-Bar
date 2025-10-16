using Microsoft.EntityFrameworkCore;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;
using GunterBar.Infrastructure.Data;

namespace GunterBar.Infrastructure.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly GunterBarDbContext _context;

    public OrderItemRepository(GunterBarDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<OrderItem?> GetByIdAsync(int id)
    {
        return await _context.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.Drink)
                .ThenInclude(d => d.Ingredients)
            .AsNoTracking()
            .FirstOrDefaultAsync(oi => oi.Id == id);
    }

    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
    {
        return await _context.OrderItems
            .Include(oi => oi.Drink)
                .ThenInclude(d => d.Ingredients)
            .Where(oi => oi.OrderId == orderId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<OrderItem> CreateAsync(OrderItem orderItem)
    {
        if (orderItem == null)
            throw new ArgumentNullException(nameof(orderItem));

        // Verificar que existe la orden
        var orderExists = await _context.Orders.AnyAsync(o => o.Id == orderItem.OrderId);
        if (!orderExists)
            throw new KeyNotFoundException($"Orden con ID {orderItem.OrderId} no encontrada");

        // Verificar que existe la bebida y tiene stock suficiente
        var drink = await _context.Drinks.FindAsync(orderItem.DrinkId);
        if (drink == null)
            throw new KeyNotFoundException($"Bebida con ID {orderItem.DrinkId} no encontrada");

        if (drink.Stock < orderItem.Quantity)
            throw new InvalidOperationException($"Stock insuficiente para la bebida {drink.Name}");

        // Actualizar el stock
        drink.UpdateStock(-orderItem.Quantity);

        await _context.OrderItems.AddAsync(orderItem);
        await _context.SaveChangesAsync();
        return orderItem;
    }

    public async Task<OrderItem> UpdateAsync(OrderItem orderItem)
    {
        if (orderItem == null)
            throw new ArgumentNullException(nameof(orderItem));

        var existingItem = await _context.OrderItems
            .Include(oi => oi.Drink)
            .FirstOrDefaultAsync(oi => oi.Id == orderItem.Id);

        if (existingItem == null)
            throw new KeyNotFoundException($"Item de orden con ID {orderItem.Id} no encontrado");

        // Si cambió la cantidad, actualizar el stock
        if (existingItem.Quantity != orderItem.Quantity)
        {
            var drink = await _context.Drinks.FindAsync(existingItem.DrinkId);
            if (drink != null)
            {
                var difference = existingItem.Quantity - orderItem.Quantity;
                if (difference < 0 && Math.Abs(difference) > drink.Stock)
                    throw new InvalidOperationException($"Stock insuficiente para la bebida {drink.Name}");
                
                drink.UpdateStock(difference);
            }
        }

        _context.Entry(existingItem).CurrentValues.SetValues(orderItem);
        await _context.SaveChangesAsync();
        return existingItem;
    }

    public async Task DeleteAsync(int id)
    {
        var orderItem = await _context.OrderItems
            .Include(oi => oi.Drink)
            .FirstOrDefaultAsync(oi => oi.Id == id);

        if (orderItem == null)
            throw new KeyNotFoundException($"Item de orden con ID {id} no encontrado");

        // Devolver el stock
        var drink = await _context.Drinks.FindAsync(orderItem.DrinkId);
        if (drink != null)
        {
            drink.UpdateStock(orderItem.Quantity);
        }

        _context.OrderItems.Remove(orderItem);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.OrderItems.AnyAsync(oi => oi.Id == id);
    }
}
