using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;
[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class CommandesController(AppDbContext db): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Commande>>> GetAll()
        => Ok(await db.Commandes.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Commande>> GetById(int id)
    {
        var commande = await db.Commandes
        .Include(c => c.Client)
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == id);

        if (commande is null) return NotFound();

        return Ok(commande);
    }

    [HttpPost]
    public async Task<ActionResult<Commande>> Create([FromBody] CommandeCreateDto dto)
    {
        var existsClient = await db.Clients.AnyAsync(c => c.Id == dto.ClientId);
        if (!existsClient) return ValidationProblem($"ClientId {dto.ClientId} n'existe pas.");

        var entity = new Commande
        {
            NumeroCommande = dto.NumeroCommande,
            MontantTotal = dto.MontantTotal,
            Statut = dto.Statut,
            ClientId = dto.ClientId
        };

        db.Commandes.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommandeUpdateDto dto)
    {
        var commande = await db.Commandes.FindAsync(id);
        if (commande is null) return NotFound();

        commande.MontantTotal = dto.MontantTotal;
        commande.Statut = dto.Statut;

        await db.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async  Task<IActionResult> Delete(int id)
    {
        var commande = await db.Commandes.FindAsync(id);
        if (commande is null) return NotFound();

        db.Commandes.Remove(commande);
        await db.SaveChangesAsync();
        return NoContent();
    }
}