using BarGunter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Contracts.IRepositories;

public interface IProductoRepository
{
    Task<List<Producto>> GetAllProductos();
    Task<Producto?> GetProductoById(int id);
    Task<int> AddProducto(Producto producto);
    Task<bool> UpdateProducto(Producto producto);
    Task<bool> DeleteProducto(int id);
}