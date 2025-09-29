using BarGunter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Contracts.IServices;

public interface ITicketService
{
    Task<List<Ticket>> GetAllTickets();
    Task<Ticket> GetTicketById(int id);
    Task<int> CreateTicket(Ticket ticket);
}