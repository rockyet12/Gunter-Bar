using BarGunter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Contracts.IServices;

public interface ICategoriaService
{
    Task<List<Categoria>> GetAllCategorias();
    Task<Categoria> GetCategoriaById(int id);
    Task<int> AddCategoria(Categoria categoria);
    Task<bool> UpdateCategoria(Categoria categoria);
    Task<bool> DeleteCategoria(int id);
}