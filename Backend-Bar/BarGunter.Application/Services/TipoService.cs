using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;

namespace BarGunter.Application.Services;

public class TipoService : ITipoService
{
    private readonly ITipoRepository _tipoRepository;

    public TipoService(ITipoRepository tipoRepository)
    {
        _tipoRepository = tipoRepository;
    }

    public async Task<List<Tipo>> GetAllTipos()
    {
        return await _tipoRepository.GetAllTipos();
    }

    public async Task<Tipo> GetTipoById(int id)
    {
        return await _tipoRepository.GetTipoById(id);
    }

    public async Task<int> AddTipo(Tipo tipo)
    {
        return await _tipoRepository.AddTipo(tipo);
    }

    public async Task<bool> UpdateTipo(Tipo tipo)
    {
        return await _tipoRepository.UpdateTipo(tipo);
    }

    public async Task<bool> DeleteTipo(int id)
    {
        return await _tipoRepository.DeleteTipo(id);
    }
}