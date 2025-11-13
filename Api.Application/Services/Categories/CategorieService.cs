namespace Api.Application.Services.Categories;

using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Api.Databases.UnitOfWork;

public class CategorieService(IUnitOfWork unitOfWork): ICategorieService
{
    private readonly IUnitOfWork _unityOfWork = unitOfWork;

    public async Task<IEnumerable<Categorie>> GetAllAsync()
    {
        return await _unityOfWork.Categories.GetAllAsync();
    }

    public async Task<Categorie?> GetByIdAsync(int id)
    {
        return await _unityOfWork.Categories.GetByIdAsync(id);
    }

    public async Task<Categorie> CreateAsync(CategorieBaseDto dto)
    {
        var categorie = new Categorie
        {
            Nom = dto.Nom,
            Description = dto.Description
        };

        await _unityOfWork.Categories.AddAsync(categorie);
        await _unityOfWork.SaveChangesAsync();
        return categorie;
    }

    public async Task<bool> UpdateAsync(int id, CategorieBaseDto dto)
    {
        var categorie = await _unityOfWork.Categories.GetByIdAsync(id);
        if (categorie is null) return false;

        categorie.Nom = dto.Nom;
        categorie.Description = dto.Description;

        _unityOfWork.Categories.UpdateAsync(categorie);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var categorie = await _unityOfWork.Categories.GetByIdAsync(id);
        if (categorie is null) return false;

        _unityOfWork.Categories.DeleteAsync(categorie);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }
}