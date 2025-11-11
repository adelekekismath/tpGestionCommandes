namespace Api.Application.Services.Categories;

using Api.Domain.Entities;
using Api.ViewModel.DTOs;

public interface ICategorieService
{
    Task<IEnumerable<Categorie>> GetAllAsync();
    Task<Categorie?> GetByIdAsync(int id);
    Task<Categorie> CreateAsync(CategorieBaseDto dto);
    Task<bool> UpdateAsync(int id, CategorieBaseDto dto);
    Task<bool> DeleteAsync(int id);
}