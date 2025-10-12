using GunterBar.Application.DTOs;
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

    public DrinksController(IDrinkService drinkService)
    {
        _drinkService = drinkService;
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
    /// Crea una nueva bebida (Solo Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
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
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<DrinkDto>>> Update(int id, [FromBody] CreateDrinkDto updateDrinkDto)
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

        var result = await _drinkService.UpdateAsync(id, updateDrinkDto);
        
        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Elimina una bebida (Solo Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
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
