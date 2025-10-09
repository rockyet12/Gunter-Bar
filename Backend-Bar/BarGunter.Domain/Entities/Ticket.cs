using System;

namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Ticket
{
    [Key]
    public int TicketId { get; set; }
    public int TableNumber { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsActive { get; set; }

    public Ticket()
    {
        CreatedDate = DateTime.Now;
        TotalAmount = 0;
        IsActive = true;
    }

    public Ticket(int ticketId, int tableNumber, string customerName, DateTime createdDate, decimal totalAmount, bool isActive)
    {
        TicketId = ticketId;
        TableNumber = tableNumber;
        CustomerName = customerName;
        CreatedDate = createdDate;
        TotalAmount = totalAmount;
        IsActive = isActive;
    }
}