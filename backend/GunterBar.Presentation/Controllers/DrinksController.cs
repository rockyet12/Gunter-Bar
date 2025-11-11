using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs;
using GunterBar.Application.DTOs.Drinks;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GunterBar.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrinksController : ControllerBase
{
    private readonly IDrinkService _drinkService;
    private readonly ILogger<DrinksController> _logger;

    public DrinksController(IDrinkService drinkService, ILogger<DrinksController> logger)
    {
        _drinkService = drinkService ?? throw new ArgumentNullException(nameof(drinkService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene todas las bebidas
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<DrinkDto>>>> GetAll()
    {
        var result = await _drinkService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Obtiene bebidas por tipo
    /// </summary>
    [HttpGet("type/{type}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<DrinkDto>>>> GetByType(DrinkType type)
    {
        var result = await _drinkService.GetByTypeAsync(type);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene una bebida específica por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DrinkDto>>> GetById(int id)
    {
        var result = await _drinkService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Crea una nueva bebida (Solo Admin y Seller)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<ActionResult<ApiResponse<DrinkDto>>> Create([FromBody] CreateDrinkDto createDrinkDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<DrinkDto>
            {
                Success = false,
                Message = "Datos inválidos",
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            });
        }

        var result = await _drinkService.CreateAsync(createDrinkDto);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Actualiza una bebida existente (Solo Admin)
    /// </summary>
    /// <summary>
    /// Actualiza una bebida existente (Solo Admin y Seller)
    /// </summary>
    /// <param name="id">ID de la bebida a actualizar</param>
    /// <param name="updateDto">Datos de actualización de la bebida</param>
    /// <returns>La bebida actualizada</returns>
    /// <response code="200">La bebida se actualizó correctamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="404">Bebida no encontrada</response>
    /// <response code="409">Conflicto con datos existentes</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Seller")]
    [ProducesResponseType(typeof(ApiResponse<DrinkDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<DrinkDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<DrinkDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<DrinkDto>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<DrinkDto>>> Update(int id, [FromBody] UpdateDrinkDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<DrinkDto>
            {
                Success = false,
                Message = "Datos inválidos",
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            });
        }

        try 
        {
            var result = await _drinkService.UpdateAsync(id, updateDto);
            
            if (!result.Success)
            {
                // Determinar el tipo de error basado en el mensaje
                if (result.Message.Contains("no encontrad", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result);
                if (result.Message.Contains("existe", StringComparison.OrdinalIgnoreCase))
                    return Conflict(result);
                
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la bebida {DrinkId}", id);
            return StatusCode(500, new ApiResponse<DrinkDto>
            {
                Success = false,
                Message = "Error interno del servidor al actualizar la bebida",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Elimina una bebida (Solo Admin y Seller)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _drinkService.DeleteAsync(id);
        
        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}
