namespace Api.Application.Services.Clients;

using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using Api.ViewModel.DTOs;
using Api.Databases.UnitOfWork;

public class ClientService(IUnitOfWork unitOfWork) : IClientService
{
    private readonly IUnitOfWork _unityOfWork = unitOfWork;

    public async Task<Client?> GetByIdAsync(int id)
        => await _unityOfWork.Clients.GetByIdAsync(id);

    public async Task<IEnumerable<Client>> GetAllAsync()
        => await _unityOfWork.Clients.GetAllAsync();

    public async Task<Client> CreateAsync(Client client)
    {
        await _unityOfWork.Clients.AddAsync(client);
        await _unityOfWork.SaveChangesAsync();
        return client;
    }

    public async Task<Client?> UpdateAsync(int id, ClientBaseDto dto)
    {
        var client = await _unityOfWork.Clients.GetByIdAsync(id);
        if (client is null) return null;

        client.Nom = dto.Nom;
        client.Prenom = dto.Prenom;
        client.Email = dto.Email;
        client.Telephone = dto.Telephone;
        client.Adresse = dto.Adresse;

        _unityOfWork.Clients.UpdateAsync(client);
        await _unityOfWork.SaveChangesAsync();

        return client;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = await _unityOfWork.Clients.GetByIdAsync(id);
        if (client is null) return false;

        _unityOfWork.Clients.UpdateAsync(client);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }
}
