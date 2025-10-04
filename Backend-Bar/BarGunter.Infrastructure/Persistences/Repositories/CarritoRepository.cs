using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Infrastructure.Persistences.Repositories
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly BarGunterDbContext _context;
        public CarritoRepository(BarGunterDbContext context)
        {
            _context = context;
        }
        public async Task<List<Carrito>> GetAllCarritos() => await _context.Carritos.ToListAsync();
        public async Task<Carrito?> GetCarritoById(int id) => await _context.Carritos.FindAsync(id);
        public async Task<int> AddCarrito(Carrito carrito)
        {
            _context.Carritos.Add(carrito);
            await _context.SaveChangesAsync();
            return carrito.IdCarrito;
        }
        public async Task<bool> UpdateCarrito(Carrito carrito)
        {
            _context.Carritos.Update(carrito);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteCarrito(int id)
        {
            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito == null) return false;
            _context.Carritos.Remove(carrito);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
