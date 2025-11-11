namespace Api.Application.Services.Produits;
using Api.Domain.Entities;
using Api.ViewModel.DTOs;

public interface IProduitService
{
    Task<IEnumerable<Produit>> GetAllAsync();
    Task<Produit?> GetByIdAsync(int id);
    Task<Produit> CreateAsync(ProduitBaseDto dto);
    Task<bool> UpdateAsync(int id, ProduitBaseDto dto);
    Task<bool> DeleteAsync(int id);
}