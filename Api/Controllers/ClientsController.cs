using Api.Contracts;
using Api.Domain.Entities;
using Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetAll()
        => Ok(await db.Clients.AsNoTracking().ToListAsync());


    [HttpGet("{id:int}")]
    public async Task<ActionResult<Client>> GetById(int id)
    {
        var client = await db.Clients
            .Include(c => c.Commandes)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
        return client is null ? NotFound() : Ok(client);
    }

    [HttpPost]
    public async Task<ActionResult<Client>> Create([FromBody] ClientCreateDto dto)
    {
        var entity = new Client
        {
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Adresse = dto.Adresse,
        };

        db.Clients.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClientUpdateDto dto)
    {
        var client = await db.Clients.FindAsync(id);
        if (client is null) return NotFound();
        client.Nom = dto.Nom; client.Prenom = dto.Prenom;
        client.Email = dto.Email; client.Telephone = dto.Telephone; client.Adresse = dto.Adresse;
        await db.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var client = await db.Clients.FindAsync(id);
        if (client is null) return NotFound();

        db.Clients.Remove(client);
        await db.SaveChangesAsync();
        return NoContent();
    }


}