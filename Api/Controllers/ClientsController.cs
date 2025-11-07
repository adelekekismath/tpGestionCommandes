using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetAll()
    {
        return Ok(await db.Clients.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Client>> GetById(int id)
    {
        var client = await db.Clients
        .Include(c => c.Commandes)
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == id);

        return client != null ? Ok(client) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Client>> Create([FromBody] ClientBaseDto dto)
    {
        var newClient = new Client
        {
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            Adresse = dto.Adresse,
            Email = dto.Email,
            Telephone = dto.Telephone,
        };

        db.Clients.Add(newClient);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = newClient.Id }, newClient);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClientBaseDto dto)
    {
        var clientToUpdate = await db.Clients.FindAsync(id);
        if (clientToUpdate != null)
        {
            clientToUpdate.Nom = dto.Nom;
            clientToUpdate.Prenom = dto.Prenom;
            clientToUpdate.Adresse = dto.Adresse;
            clientToUpdate.Email = dto.Email;
            clientToUpdate.Telephone = dto.Telephone;

            await db.SaveChangesAsync();
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var clientToDelete = await db.Clients.FindAsync(id);
        if (clientToDelete is null) return NotFound();

        db.Clients.Remove(clientToDelete);
        await db.SaveChangesAsync();
        return NoContent();
    }


}