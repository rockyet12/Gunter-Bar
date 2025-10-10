using BarGunter.Application.Interfaces.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarGunter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrinkController : ControllerBase
{
    private readonly IDrinkService _drinkService;

    public DrinkController(IDrinkService drinkService)
    {
        _drinkService = drinkService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Drink>>> GetAll()
    {
        var drinks = await _drinkService.GetAllAsync();
        return Ok(drinks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Drink>> GetById(int id)
    {
        var drink = await _drinkService.GetByIdAsync(id);
        if (drink == null)
            return NotFound();
        return Ok(drink);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<Drink>> Create([FromBody] Drink drink)
    {
        var createdDrink = await _drinkService.CreateAsync(drink);
        return CreatedAtAction(nameof(GetById), new { id = createdDrink.DrinkId }, createdDrink);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<Drink>> Update(int id, [FromBody] Drink drink)
    {
        var updatedDrink = await _drinkService.UpdateAsync(id, drink);
        if (updatedDrink == null)
            return NotFound();
        return Ok(updatedDrink);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _drinkService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }

    [HttpGet("type/{drinkTypeId}")]
    public async Task<ActionResult<IEnumerable<Drink>>> GetByType(int drinkTypeId)
    {
        var drinks = await _drinkService.GetAllAsync();
        // Filtrar por tipo según la lógica de negocio
        return Ok(drinks);
    }

    [HttpGet("alcoholic")]
    public async Task<ActionResult<IEnumerable<Drink>>> GetAlcoholic()
    {
        var drinks = await _drinkService.GetAllAsync();
        // Filtrar bebidas alcohólicas según la lógica de negocio
        return Ok(drinks);
    }

    [HttpGet("non-alcoholic")]
    public async Task<ActionResult<IEnumerable<Drink>>> GetNonAlcoholic()
    {
        var drinks = await _drinkService.GetAllAsync();
        // Filtrar bebidas no alcohólicas según la lógica de negocio
        return Ok(drinks);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Drink>>> Search([FromQuery] string query)
    {
        var drinks = await _drinkService.GetAllAsync();
        // Implementar búsqueda por nombre según la lógica de negocio
        return Ok(drinks);
    }

    [HttpGet("popular")]
    public async Task<ActionResult<IEnumerable<Drink>>> GetPopular()
    {
        var drinks = await _drinkService.GetAllAsync();
        // Filtrar bebidas populares según la lógica de negocio
        return Ok(drinks);
    }
}
