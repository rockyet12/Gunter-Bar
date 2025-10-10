using BarGunter.Application.Interfaces.IRepositories;
using BarGunter.Domain.Entities;
using BarGunter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BarGunter.Infrastructure.Repositories;
    public class CartRepository : ICartRepository
    {
        private readonly BarGunterDbContext _context;

        public CartRepository(BarGunterDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _context.Carts.FindAsync(id);
        }

        public async Task<Cart> CreateAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart?> UpdateAsync(int id, Cart cart)
        {
            var existingCart = await _context.Carts.FindAsync(id);
            if (existingCart == null) return null;

            existingCart.ProductId = cart.ProductId;
            existingCart.Quantity = cart.Quantity;
            existingCart.Price = cart.Price;

            await _context.SaveChangesAsync();
            return existingCart;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) return false;

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }
    }

