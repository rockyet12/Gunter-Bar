using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _carritoRepository;
        public CarritoService(ICarritoRepository carritoRepository)
        {
            _carritoRepository = carritoRepository;
        }
        public async Task<List<Carrito>> GetAllCarritos() => await _carritoRepository.GetAllCarritos();
        public async Task<Carrito?> GetCarritoById(int id) => await _carritoRepository.GetCarritoById(id);
        public async Task<int> AddCarrito(Carrito carrito) => await _carritoRepository.AddCarrito(carrito);
        public async Task<bool> UpdateCarrito(Carrito carrito) => await _carritoRepository.UpdateCarrito(carrito);
        public async Task<bool> DeleteCarrito(int id) => await _carritoRepository.DeleteCarrito(id);
    }
}
