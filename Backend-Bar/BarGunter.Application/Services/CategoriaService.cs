using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<List<Categoria>> GetAllCategorias()
    {
        return await _categoriaRepository.GetAllCategorias();
    }

    public async Task<Categoria> GetCategoriaById(int id)
    {
        return await _categoriaRepository.GetCategoriaById(id);
    }

    public async Task<int> AddCategoria(Categoria categoria)
    {
        return await _categoriaRepository.AddCategoria(categoria);
    }

    public async Task<bool> UpdateCategoria(Categoria categoria)
    {
        return await _categoriaRepository.UpdateCategoria(categoria);
    }

    public async Task<bool> DeleteCategoria(int id)
    {
        return await _categoriaRepository.DeleteCategoria(id);
    }
}