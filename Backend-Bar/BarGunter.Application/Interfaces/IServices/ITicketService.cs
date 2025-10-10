using BarGunter.Domain.Entities;

namespace BarGunter.Application.Contracts.IServices;
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<Ticket?> UpdateAsync(int id, Ticket ticket);
        Task<bool> DeleteAsync(int id);
    }

