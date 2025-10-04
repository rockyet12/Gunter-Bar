using BarGunter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Contracts.IServices
{
    public interface ICarritoService
    {
        Task<List<Carrito>> GetAllCarritos();
        Task<Carrito?> GetCarritoById(int id);
        Task<int> AddCarrito(Carrito carrito);
        Task<bool> UpdateCarrito(Carrito carrito);
        Task<bool> DeleteCarrito(int id);
    }
}
