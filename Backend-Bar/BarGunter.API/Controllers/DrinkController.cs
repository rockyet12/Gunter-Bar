using BarGunter.Application.Interfaces.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarGunter.API.Controllers;

/// <summary>
/// Controller para gestión de bebidas del bar
/// Versión mejorada con funcionalidades de filtrado por tipo y alcohol
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DrinkController : ControllerBase
{
    private readonly IDrinkService _drinkService;
    private readonly IDrinkTypeService _drinkTypeService;
    private readonly ILogger<DrinkController> _logger;

    public DrinkController(
        IDrinkService drinkService, 
        IDrinkTypeService drinkTypeService,
        ILogger<DrinkController> logger)
    {
        _drinkService = drinkService;
        _drinkTypeService = drinkTypeService;
        _logger = logger;
    }

    /// <summary>
    /// Obtener todas las bebidas con filtros avanzados
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Drink>>> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] int? typeId = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null)
    {
        _logger.LogInformation("Obteniendo bebidas con filtros: search={Search}, typeId={TypeId}, minPrice={MinPrice}, maxPrice={MaxPrice}",
            search, typeId, minPrice, maxPrice);

        var drinks = await _drinkService.GetAllAsync();

        // Aplicar filtros
        if (!string.IsNullOrEmpty(search))
        {
            drinks = drinks.Where(d => 
                (d.Name?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (d.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        if (typeId.HasValue)
        {
            drinks = drinks.Where(d => d.TypeId == typeId.Value);
        }

        if (minPrice.HasValue)
        {
            drinks = drinks.Where(d => d.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            drinks = drinks.Where(d => d.Price <= maxPrice.Value);
        }

        var result = drinks.ToList();
        _logger.LogInformation("Bebidas filtradas: {Count} encontradas", result.Count);
        
        return Ok(result);
    }

    /// <summary>
    /// Obtener bebidas por tipo específico
    /// </summary>
    [HttpGet("by-type/{typeId}")]
    public async Task<ActionResult<IEnumerable<Drink>>> GetByType(int typeId)
    {
        _logger.LogInformation("Obteniendo bebidas de tipo {TypeId}", typeId);
        
        var drinks = await _drinkService.GetAllAsync();
        var typeDrinks = drinks.Where(d => d.TypeId == typeId).ToList();
        
        _logger.LogInformation("Bebidas de tipo {TypeId} encontradas: {Count}", typeId, typeDrinks.Count);
        return Ok(typeDrinks);
    }

    /// <summary>
    /// Obtener bebidas más caras (premium)
    /// </summary>
    [HttpGet("premium")]
    public async Task<ActionResult<IEnumerable<Drink>>> GetPremiumDrinks([FromQuery] int limit = 5)
    {
        _logger.LogInformation("Obteniendo bebidas premium, límite: {Limit}", limit);
        
        var drinks = await _drinkService.GetAllAsync();
        
        // Consideramos premium las bebidas más caras
        var premiumDrinks = drinks
            .OrderByDescending(d => d.Price)
            .Take(limit)
            .ToList();

        _logger.LogInformation("Bebidas premium obtenidas: {Count}", premiumDrinks.Count);
        return Ok(premiumDrinks);
    }

    /// <summary>
    /// Obtener bebidas populares del día
    /// </summary>
    [HttpGet("popular-today")]
    public async Task<ActionResult<IEnumerable<Drink>>> GetPopularDrinks([FromQuery] int limit = 5)
    {
        _logger.LogInformation("Obteniendo bebidas populares del día, límite: {Limit}", limit);
        
        var drinks = await _drinkService.GetAllAsync();
        
        // Simulamos popularidad rotando entre diferentes bebidas
        var popularDrinks = drinks
            .OrderBy(d => d.DrinkId) // Ordenar por ID para consistencia
            .Take(limit)
            .ToList();

        _logger.LogInformation("Bebidas populares del día obtenidas: {Count}", popularDrinks.Count);
        return Ok(popularDrinks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Drink>> GetById(int id)
    {
        _logger.LogInformation("Obteniendo bebida {DrinkId}", id);
        
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
        _logger.LogInformation("Eliminando bebida {DrinkId}", id);
        
        var deleted = await _drinkService.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("No se pudo eliminar bebida {DrinkId} - no encontrada", id);
            return NotFound($"Bebida con ID {id} no encontrada");
        }
        _logger.LogInformation("Bebida {DrinkId} eliminada exitosamente", id);
        return NoContent();
    }

    /// <summary>
    /// Obtener estadísticas de bebidas
    /// </summary>
    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<object>> GetDrinkStats()
    {
        _logger.LogInformation("Obteniendo estadísticas de bebidas");
        
        var drinks = await _drinkService.GetAllAsync();
        var drinkTypes = await _drinkTypeService.GetAllAsync();
        
        var stats = new
        {
            TotalDrinks = drinks.Count(),
            AveragePrice = drinks.Any() ? drinks.Average(d => d.Price) : 0,
            MaxPrice = drinks.Any() ? drinks.Max(d => d.Price) : 0,
            MinPrice = drinks.Any() ? drinks.Min(d => d.Price) : 0,
            TotalTypes = drinkTypes.Count(),
            DrinksByType = drinkTypes.Select(dt => new
            {
                TypeName = dt.Name,
                DrinkCount = drinks.Count(d => d.TypeId == dt.TypeId)
            }).ToList()
        };
        
        _logger.LogInformation("Estadísticas de bebidas generadas: {TotalDrinks} bebidas totales", stats.TotalDrinks);
        return Ok(stats);
    }
}
