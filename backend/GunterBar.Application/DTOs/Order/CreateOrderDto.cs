namespace GunterBar.Application.DTOs.Order;

public class CreateOrderDto
{
    public int UserId { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}
