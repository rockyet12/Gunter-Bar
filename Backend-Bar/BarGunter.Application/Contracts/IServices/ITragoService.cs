using BarGunter.Domain.Entities;
namespace BarGunter.Application.Contracts.IServices;

public interface ITragoService
{
    Task<List<Tragos>> GetAllTragos();
    Task<Tragos> GetTragoById(int id);
    Task<int> AddTrago(Tragos trago);
    Task<bool> UpdateTrago(Tragos trago);
    Task<bool> DeleteTrago(int id);
}