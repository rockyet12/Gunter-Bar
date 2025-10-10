using BarGunter.Domain.Entities;

namespace BarGunter.Application.Interfaces.IRepositories;
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<Ticket?> UpdateAsync(int id, Ticket ticket);
        Task<bool> DeleteAsync(int id);
    }

