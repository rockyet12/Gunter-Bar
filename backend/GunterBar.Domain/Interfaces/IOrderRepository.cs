using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;
using GunterBar.Domain.Entities;

namespace GunterBar.Domain.Interfaces;

// Contrato para repositorio de órdenes
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
    Task<Order> CreateAsync(Order order);
    Task<Order> UpdateAsync(Order order);
    Task DeleteAsync(int id);
}
