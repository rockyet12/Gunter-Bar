using GunterBar.Domain.Enums;

namespace GunterBar.Application.DTOs.Order;

public class UpdateOrderStatusDto
{
    public int OrderId { get; set; }
    public OrderStatus NewStatus { get; set; }
}
