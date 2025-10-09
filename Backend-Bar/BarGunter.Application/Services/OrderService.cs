using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;

namespace BarGunter.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task<Order> CreateAsync(Order order)
    {
        return await _orderRepository.CreateAsync(order);
    }

    public async Task<Order?> UpdateAsync(int id, Order order)
    {
        return await _orderRepository.UpdateAsync(id, order);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _orderRepository.DeleteAsync(id);
    }
}

