using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BarGunter.Application.Contracts.IServices;

public interface IProductoService 
{
    Task<List<Producto>> GetAllProductos();
    Task<Producto?> GetProductoById(int id);
    Task<int> AddProducto(Producto producto);
    Task<bool> UpdateProducto(Producto producto);
    Task<bool> DeleteProducto(int id);

}
