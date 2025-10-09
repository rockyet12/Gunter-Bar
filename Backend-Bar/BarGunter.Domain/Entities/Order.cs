using BarGunter.Domain.Enums;

namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Order
{
    [Key]
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public int TicketId { get; set; }

    // Navigation property
    public Ticket? Ticket { get; set; }

    public Order()
    {
        OrderDate = DateTime.Now;
        Status = OrderStatus.Pending;
    }

    public Order(int orderId, DateTime orderDate, decimal totalAmount, OrderStatus status, int ticketId)
    {
        OrderId = orderId;
        OrderDate = orderDate;
        TotalAmount = totalAmount;
        Status = status;
        TicketId = ticketId;
    }
}
