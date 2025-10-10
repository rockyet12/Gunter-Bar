using BarGunter.Application.Interfaces.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarGunter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
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
        var deleted = await _categoryService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
