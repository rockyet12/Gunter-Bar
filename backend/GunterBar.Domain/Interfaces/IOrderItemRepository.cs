using GunterBar.Domain.Entities;

namespace GunterBar.Domain.Interfaces;

public interface IOrderItemRepository
{
    Task<OrderItem?> GetByIdAsync(int id);
    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
    Task<OrderItem> CreateAsync(OrderItem orderItem);
    Task<OrderItem> UpdateAsync(OrderItem orderItem);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
