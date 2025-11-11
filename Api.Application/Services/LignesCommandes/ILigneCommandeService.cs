namespace Api.Application.Services.LignesCommandes;
using Api.Domain.Entities;
using Api.ViewModel.DTOs;

public interface ILigneCommandeService
{
    Task<IEnumerable<LigneCommande>> GetAllAsync();
    Task<LigneCommande?> GetByIdAsync(int id);
    Task<LigneCommande> CreateAsync(LigneCommandeCreateDto dto);
    Task<bool> UpdateAsync(int id, LigneCommandeUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}