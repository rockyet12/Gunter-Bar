using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BarGunter.Application.Contracts.IRepositories;

public interface IProductoRepository
{
    Task<List<Producto>> GetAllProductos();
    Task<Producto> GetproductosById(int CDProducto);
    Task<int> AddProducto(Producto producto);
    Task<bool> Updateproducto(Producto producto);
    Task<bool> DeleteProducto(int CDProducto);

}
