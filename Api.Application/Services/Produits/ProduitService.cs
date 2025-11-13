namespace Api.Application.Services.Produits;

using Api.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.UnitOfWork;
public class ProduitService( IUnitOfWork unitOfWork ) : IProduitService
{
    private readonly IUnitOfWork _unityOfWork = unitOfWork;

    public async Task<Produit> CreateAsync(ProduitBaseDto dto)
    {
        var produit = new Produit
        {
            Nom = dto.Nom,
            Description = dto.Description,
            Prix = dto.Prix,
            Stock = dto.Stock,
            CategorieId = dto.CategorieId
        };

        await _unityOfWork.Produits.AddAsync(produit);
        await _unityOfWork.SaveChangesAsync();
        return produit;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var produit = await _unityOfWork.Produits.GetByIdAsync(id);
        if (produit == null) return false;

        _unityOfWork.Produits.DeleteAsync(produit);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Produit>> GetAllAsync()
    {
        return await _unityOfWork.Produits.GetAllAsync();
    }

    public async Task<Produit?> GetByIdAsync(int id)
    {
        return await _unityOfWork.Produits.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(int id, ProduitBaseDto dto)
    {
        var produit = await _unityOfWork.Produits.GetByIdAsync(id);
        if (produit == null) return false;

        produit.Nom = dto.Nom;
        produit.Description = dto.Description;
        produit.Prix = dto.Prix;
        produit.Stock = dto.Stock;
        produit.CategorieId = dto.CategorieId;

        _unityOfWork.Produits.UpdateAsync(produit);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }
}