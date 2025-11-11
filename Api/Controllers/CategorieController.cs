namespace Api.Controllers;

using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Application.Services.Categories;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategorieController: ControllerBase
{
    private readonly ICategorieService _categorieService;

    public CategorieController(ICategorieService categorieService)
    {
        _categorieService = categorieService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categorieService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categorie = await _categorieService.GetByIdAsync(id);
        if (categorie == null)
        {
            return NotFound();
        }
        return Ok(categorie);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategorieBaseDto dto)
    {
        var createdCategorie = await _categorieService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdCategorie.Id }, createdCategorie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategorieBaseDto dto)
    {
        var updated = await _categorieService.UpdateAsync(id, dto);
        if (!updated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _categorieService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}