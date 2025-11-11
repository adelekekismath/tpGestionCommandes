namespace Api.Controllers;

using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Application.Services.LignesCommandes;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LigneCommandeController : ControllerBase
{
    private readonly ILigneCommandeService _ligneCommandeService;

    public LigneCommandeController(ILigneCommandeService ligneCommandeService)
    {
        _ligneCommandeService = ligneCommandeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lignesCommandes = await _ligneCommandeService.GetAllAsync();
        return Ok(lignesCommandes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ligneCommande = await _ligneCommandeService.GetByIdAsync(id);
        if (ligneCommande == null)
        {
            return NotFound();
        }
        return Ok(ligneCommande);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LigneCommandeCreateDto dto)
    {
        var createdLigneCommande = await _ligneCommandeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdLigneCommande.Id }, createdLigneCommande);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LigneCommandeUpdateDto dto)
    {
        var updated = await _ligneCommandeService.UpdateAsync(id, dto);
        if (!updated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _ligneCommandeService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}