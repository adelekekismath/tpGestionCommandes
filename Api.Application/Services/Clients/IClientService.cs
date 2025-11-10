namespace Api.Application.Services.Clients;

using Api.Domain.Entities;
using Api.ViewModel.DTOs;

public interface IClientService
{
    Task<Client?> GetByIdAsync(int id);
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client> CreateAsync(Client client);
    Task<Client?> UpdateAsync(int id,ClientBaseDto client);
    Task<bool> DeleteAsync(int id);
}