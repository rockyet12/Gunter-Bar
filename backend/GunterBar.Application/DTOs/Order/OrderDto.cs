using GunterBar.Domain.Enums;

namespace GunterBar.Application.DTOs.Order;

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? CancelledDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
