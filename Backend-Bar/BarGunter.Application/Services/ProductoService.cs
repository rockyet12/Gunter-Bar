using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<List<Producto>> GetAllProductos()
    {
        return await _productoRepository.GetAllProductos();
    }

    public async Task<Producto> GetproductosById(int CDProducto) // Coincide con tu interfaz
    {
        return await _productoRepository.GetproductosById(CDProducto);
    }

    public async Task<int> AddProducto(Producto producto)
    {
        return await _productoRepository.AddProducto(producto);
    }

    public async Task<bool> Updateproducto(Producto producto) // Coincide con tu interfaz
    {
        return await _productoRepository.Updateproducto(producto);
    }

    public async Task<bool> DeleteProducto(int CDProducto)
    {
        return await _productoRepository.DeleteProducto(CDProducto);
    }
}