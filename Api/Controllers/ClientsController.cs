using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Application.Services.Clients;

namespace Api.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController(IClientService _service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Client>> GetById(int id)
    {
        var client = await _service.GetByIdAsync(id);
        if (client is null) return NotFound();
        return Ok(client);
    }

    [HttpPost]
    public async Task<ActionResult<Client>> Create([FromBody] ClientBaseDto dto)
    {
        var client = new Client
        {
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            Adresse = dto.Adresse,
            Email = dto.Email,
            Telephone = dto.Telephone
        };

        Console.WriteLine("Creating client: " + client.Nom);

        var createdClient = await _service.CreateAsync(client);
        return CreatedAtAction(nameof(GetById), new { id = createdClient.Id }, createdClient);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClientBaseDto dto)
    {
        var updatedClient = await _service.UpdateAsync(id, dto);
        if (updatedClient is null) return NotFound();
        return Ok(updatedClient);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }


}