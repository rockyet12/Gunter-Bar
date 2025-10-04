using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Infrastructure.Persistences.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly BarGunterDbContext _context;
        public ProductoRepository(BarGunterDbContext context)
        {
            _context = context;
        }
        public async Task<List<Producto>> GetAllProductos() => await _context.Productos.ToListAsync();
        public async Task<Producto?> GetProductoById(int id) => await _context.Productos.FindAsync(id);
        public async Task<int> AddProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto.CDProducto;
        }
        public async Task<bool> UpdateProducto(Producto producto)
        {
            _context.Productos.Update(producto);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;
            _context.Productos.Remove(producto);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
