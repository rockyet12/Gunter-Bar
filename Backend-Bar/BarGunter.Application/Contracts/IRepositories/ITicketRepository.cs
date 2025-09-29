using BarGunter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Contracts.IRepositories;

public interface ITicketRepository
{
    Task<List<Ticket>> GetAllTickets();
    Task<Ticket> GetTicketById(int id);
    Task<int> AddTicket(Ticket ticket);
}