namespace Api.Application.Services.Clients;

using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using Api.ViewModel.DTOs;

public class ClientService: IClientService
{
    private readonly AppDbContext _db;
    public ClientService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<Client?> GetByIdAsync(int id)
        => await _db.Clients.FindAsync(id);

    public async Task<IEnumerable<Client>> GetAllAsync()
        => await _db.Clients.AsNoTracking().ToListAsync();

    public async Task<Client> CreateAsync(Client client)
    {
        _db.Clients.Add(client);
        await _db.SaveChangesAsync();
        return client;
    }

    public async Task<Client?> UpdateAsync(int id,ClientBaseDto client)
    {
        var existingClient = await _db.Clients.FindAsync(id);
        if (existingClient is null) return null;

        existingClient.Nom = client.Nom;
        existingClient.Email = client.Email;
        existingClient.Telephone = client.Telephone;
        existingClient.Adresse = client.Adresse;

        await _db.SaveChangesAsync();
        return existingClient;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = await _db.Clients.FindAsync(id);
        if (client is null) return false;

        _db.Clients.Remove(client);
        await _db.SaveChangesAsync();
        return true;
    }
}
