using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;
using GunterBar.Domain.Interfaces;
using GunterBar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GunterBar.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly GunterBarDbContext _context;

    public OrderRepository(GunterBarDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Drink)
                    .ThenInclude(d => d.Ingredients)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("El ID del usuario debe ser mayor que 0", nameof(userId));

        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado");

        return await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Drink)
                    .ThenInclude(d => d.Ingredients)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Drink)
                    .ThenInclude(d => d.Ingredients)
            .OrderByDescending(o => o.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Drink)
                    .ThenInclude(d => d.Ingredients)
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Order> CreateAsync(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        // Verificar existencia del usuario
        var userExists = await _context.Users.AnyAsync(u => u.Id == order.UserId);
        if (!userExists)
            throw new KeyNotFoundException($"Usuario con ID {order.UserId} no encontrado");

        // Verificar existencia de las bebidas y stock suficiente
        foreach (var item in order.Items)
        {
            var drink = await _context.Drinks.FindAsync(item.DrinkId);
            if (drink == null)
                throw new KeyNotFoundException($"Bebida con ID {item.DrinkId} no encontrada");

            if (drink.Stock < item.Quantity)
                throw new InvalidOperationException($"Stock insuficiente para la bebida {drink.Name}");

            // Actualizar el stock
            drink.UpdateStock(-item.Quantity);
        }

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        // Recargar la orden con todas sus relaciones
        await _context.Entry(order)
            .Reference(o => o.User)
            .LoadAsync();
        
        await _context.Entry(order)
            .Collection(o => o.Items)
            .Query()
            .Include(oi => oi.Drink)
            .LoadAsync();

        return order;
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        var existingOrder = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == order.Id);

        if (existingOrder == null)
            throw new KeyNotFoundException($"Orden con ID {order.Id} no encontrada");

        // Actualizar propiedades básicas
        _context.Entry(existingOrder).CurrentValues.SetValues(order);
        existingOrder.UpdatedAt = DateTime.UtcNow;

        // Actualizar items si hay cambios
        if (order.Items != null)
        {
            // Eliminar items que ya no existen
            foreach (var existingItem in existingOrder.Items.ToList())
            {
                if (!order.Items.Any(i => i.Id == existingItem.Id))
                {
                    // Devolver el stock
                    var drink = await _context.Drinks.FindAsync(existingItem.DrinkId);
                    if (drink != null)
                    {
                        drink.UpdateStock(existingItem.Quantity);
                    }
                    existingOrder.Items.Remove(existingItem);
                }
            }

            // Actualizar o agregar nuevos items
            foreach (var itemDetail in order.Items)
            {
                var existingItem = existingOrder.Items
                    .FirstOrDefault(i => i.Id == itemDetail.Id);

                if (existingItem == null)
                {
                    // Verificar stock para nuevo item
                    var drink = await _context.Drinks.FindAsync(itemDetail.DrinkId);
                    if (drink == null)
                        throw new KeyNotFoundException($"Bebida con ID {itemDetail.DrinkId} no encontrada");

                    if (drink.Stock < itemDetail.Quantity)
                        throw new InvalidOperationException($"Stock insuficiente para la bebida {drink.Name}");

                    drink.UpdateStock(-itemDetail.Quantity);
                    existingOrder.Items.Add(itemDetail);
                }
                else
                {
                    // Ajustar stock si la cantidad cambió
                    if (existingItem.Quantity != itemDetail.Quantity)
                    {
                        var drink = await _context.Drinks.FindAsync(itemDetail.DrinkId);
                        if (drink != null)
                        {
                            var difference = existingItem.Quantity - itemDetail.Quantity;
                            drink.UpdateStock(difference);
                        }
                    }
                    _context.Entry(existingItem).CurrentValues.SetValues(itemDetail);
                }
            }
        }

        await _context.SaveChangesAsync();
        return existingOrder;
    }

    public async Task DeleteAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            throw new KeyNotFoundException($"Orden con ID {id} no encontrada");

        // Devolver el stock de las bebidas
        foreach (var item in order.Items)
        {
            var drink = await _context.Drinks.FindAsync(item.DrinkId);
            if (drink != null)
            {
                drink.UpdateStock(item.Quantity);
            }
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }
}
