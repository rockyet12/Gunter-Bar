using BarGunter.Application.Interfaces.IRepositories;
using BarGunter.Application.Interfaces.IServices;
using BarGunter.Domain.Entities;

namespace BarGunter.Application.Services;
    public class DrinkTypeService : IDrinkTypeService
    {
        private readonly IDrinkTypeRepository _drinkTypeRepository;

        public DrinkTypeService(IDrinkTypeRepository drinkTypeRepository)
        {
            _drinkTypeRepository = drinkTypeRepository;
        }

        public async Task<IEnumerable<DrinkType>> GetAllAsync()
        {
            return await _drinkTypeRepository.GetAllAsync();
        }

        public async Task<DrinkType?> GetByIdAsync(int id)
        {
            return await _drinkTypeRepository.GetByIdAsync(id);
        }

        public async Task<DrinkType> CreateAsync(DrinkType drinkType)
        {
            return await _drinkTypeRepository.CreateAsync(drinkType);
        }

        public async Task<DrinkType?> UpdateAsync(int id, DrinkType drinkType)
        {
            return await _drinkTypeRepository.UpdateAsync(id, drinkType);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _drinkTypeRepository.DeleteAsync(id);
        }
    }

