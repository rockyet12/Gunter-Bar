using BarGunter.Application.Interfaces.IRepositories;
using BarGunter.Domain.Entities;
using BarGunter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BarGunter.Infrastructure.Repositories;
    public class TicketRepository : ITicketRepository
    {
        private readonly BarGunterDbContext _context;

        public TicketRepository(BarGunterDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket?> UpdateAsync(int id, Ticket ticket)
        {
            var existingTicket = await _context.Tickets.FindAsync(id);
            if (existingTicket == null) return null;

            existingTicket.TableNumber = ticket.TableNumber;
            existingTicket.CustomerName = ticket.CustomerName;
            existingTicket.CreatedDate = ticket.CreatedDate;
            existingTicket.TotalAmount = ticket.TotalAmount;
            existingTicket.IsActive = ticket.IsActive;

            await _context.SaveChangesAsync();
            return existingTicket;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }
    }

