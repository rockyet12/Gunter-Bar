using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
namespace BarGunter.Application.Services;

public class TragoService : ITragoService
{
    private readonly ITragoRepository _tragoRepository;

    public TragoService(ITragoRepository tragoRepository)
    {
        _tragoRepository = tragoRepository;
    }

    public async Task<List<Tragos>> GetAllTragos()
    {
        return await _tragoRepository.GetAllTragos();
    }

    public async Task<Tragos> GetTragoById(int id)
    {
        return await _tragoRepository.GetTragoById(id);
    }

    public async Task<int> AddTrago(Tragos trago) // <-- El método recibe y devuelve los tipos correctos
    {
        return await _tragoRepository.AddTrago(trago);
    }

    public async Task<bool> UpdateTrago(Tragos trago)
    {
        return await _tragoRepository.UpdateTrago(trago);
    }

    public async Task<bool> DeleteTrago(int id)
    {
        return await _tragoRepository.DeleteTrago(id);
    }
}