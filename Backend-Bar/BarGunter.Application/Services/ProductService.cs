using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;

namespace BarGunter.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        return await _productRepository.CreateAsync(product);
    }

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        return await _productRepository.UpdateAsync(id, product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }
}

