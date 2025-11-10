namespace Api.Application.Services.Commandes;

using Api.Domain.Entities;
using Api.ViewModel.DTOs;

public interface ICommandeService
{
    Task<Commande?> GetByIdAsync(int id);
    Task<IEnumerable<Commande>> GetAllAsync();
    Task<Commande> CreateAsync(CommandeCreateDto commande);
    Task<bool> UpdateAsync(int id,CommandeUpdateDto commande);
    Task<bool> DeleteAsync(int id);
}