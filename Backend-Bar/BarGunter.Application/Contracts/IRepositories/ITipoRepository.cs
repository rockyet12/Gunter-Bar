using BarGunter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Contracts.IRepositories;

public interface ITipoRepository
{
    Task<List<Tipo>> GetAllTipos();
    Task<Tipo> GetTipoById(int id);
    Task<int> AddTipo(Tipo tipo);
    Task<bool> UpdateTipo(Tipo tipo);
    Task<bool> DeleteTipo(int id);
}