using Microsoft.AspNetCore.Mvc;
using BarGunter.Application.Interfaces.IServices;
using BarGunter.Domain.Entities;

namespace BarGunter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrinkTypeController : ControllerBase
{
    private readonly IDrinkTypeService _drinkTypeService;

    public DrinkTypeController(IDrinkTypeService drinkTypeService)
    {
        _drinkTypeService = drinkTypeService;
    }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DrinkType>>> Get()
        {
            var drinkTypes = await _drinkTypeService.GetAllAsync();
            return Ok(drinkTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DrinkType>> GetById(int id)
        {
            var drinkType = await _drinkTypeService.GetByIdAsync(id);
            if (drinkType == null)
                return NotFound();
            return Ok(drinkType);
        }

        [HttpPost]
        public async Task<ActionResult<DrinkType>> Create([FromBody] DrinkType drinkType)
        {
            var createdDrinkType = await _drinkTypeService.CreateAsync(drinkType);
            return CreatedAtAction(nameof(GetById), new { id = createdDrinkType.TypeId }, createdDrinkType);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DrinkType>> Update(int id, [FromBody] DrinkType drinkType)
        {
            var updatedDrinkType = await _drinkTypeService.UpdateAsync(id, drinkType);
            if (updatedDrinkType == null)
                return NotFound();
            return Ok(updatedDrinkType);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _drinkTypeService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
