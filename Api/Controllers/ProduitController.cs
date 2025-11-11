namespace Api.Controllers;

using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Application.Services.Produits;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProduitController : ControllerBase
{
    private readonly IProduitService _produitService;

    public ProduitController(IProduitService produitService)
    {
        _produitService = produitService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var produits = await _produitService.GetAllAsync();
        return Ok(produits);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var produit = await _produitService.GetByIdAsync(id);
        if (produit == null)
        {
            return NotFound();
        }
        return Ok(produit);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProduitBaseDto dto)
    {
        var createdProduit = await _produitService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdProduit.Id }, createdProduit);
    }   

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProduitBaseDto dto)
    {
        var updated = await _produitService.UpdateAsync(id, dto);
        if (!updated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _produitService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}