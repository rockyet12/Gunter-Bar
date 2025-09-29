using BarGunter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Contracts.IRepositories;

public interface IProductoRepository
{
    Task<List<Producto>> GetAllProductos();
    Task<Producto> GetproductosById(int CDProducto); // Coincide con tu interfaz
    Task<int> AddProducto(Producto producto);
    Task<bool> Updateproducto(Producto producto); // Coincide con tu interfaz
    Task<bool> DeleteProducto(int CDProducto);
}