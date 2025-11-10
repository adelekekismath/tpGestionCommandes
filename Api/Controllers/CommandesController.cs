using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Application.Services.Commandes;

namespace Api.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommandesController: ControllerBase
{
    private readonly ICommandeService _service;

    public CommandesController(ICommandeService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Commande>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Commande>> GetById(int id)
    {
        var commande = await _service.GetByIdAsync(id);
        if (commande is null) return NotFound();
        return Ok(commande);
    }

    [HttpPost]
    public async Task<ActionResult<Commande>> Create([FromBody] CommandeCreateDto dto)
    {
        var createdCommande = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdCommande.Id }, createdCommande);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommandeUpdateDto dto)
    {
        var updatedCommande = await _service.UpdateAsync(id, dto);
        if (!updatedCommande) return NotFound();
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public async  Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}