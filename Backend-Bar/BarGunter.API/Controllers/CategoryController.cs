using BarGunter.Application.Interfaces.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarGunter.API.Controllers;

/// <summary>
/// Controller para gestión de categorías de productos
/// Versión mejorada con logging y validaciones adicionales
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// Obtener todas las categorías disponibles
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        _logger.LogInformation("Obteniendo todas las categorías");
        
        var categories = await _categoryService.GetAllAsync();
        
        _logger.LogInformation("Categorías obtenidas: {Count} encontradas", categories.Count());
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound();
        return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Category>> Create([FromBody] Category category)
    {
        var createdCategory = await _categoryService.CreateAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = createdCategory.CategoryId }, createdCategory);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Category>> Update(int id, [FromBody] Category category)
    {
        var updatedCategory = await _categoryService.UpdateAsync(id, category);
        if (updatedCategory == null)
            return NotFound();
        return Ok(updatedCategory);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("Eliminando categoría {CategoryId}", id);
        
        var deleted = await _categoryService.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("No se pudo eliminar categoría {CategoryId} - no encontrada", id);
            return NotFound($"Categoría con ID {id} no encontrada");
        }
        
        _logger.LogInformation("Categoría {CategoryId} eliminada exitosamente", id);
        return NoContent();
    }

    /// <summary>
    /// Buscar categorías por nombre
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Category>>> SearchCategories([FromQuery] string query)
    {
        _logger.LogInformation("Buscando categorías con query: {Query}", query);
        
        var categories = await _categoryService.GetAllAsync();
        
        if (!string.IsNullOrEmpty(query))
        {
            categories = categories.Where(c => 
                (c.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.Description?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        var result = categories.ToList();
        _logger.LogInformation("Búsqueda de categorías completada: {Count} encontradas", result.Count);
        
        return Ok(result);
    }

    /// <summary>
    /// Obtener estadísticas de categorías
    /// </summary>
    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<object>> GetCategoryStats()
    {
        _logger.LogInformation("Obteniendo estadísticas de categorías");
        
        var categories = await _categoryService.GetAllAsync();
        
        var stats = new
        {
            TotalCategories = categories.Count(),
            CategoriesWithProducts = 0, // Se podría implementar con ProductService
            AverageProductsPerCategory = 0.0, // Se podría implementar con ProductService
            LastCreatedCategory = categories.OrderByDescending(c => c.CategoryId).FirstOrDefault()?.Name ?? "N/A"
        };
        
        _logger.LogInformation("Estadísticas de categorías generadas: {Total} categorías totales", stats.TotalCategories);
        return Ok(stats);
    }
}
