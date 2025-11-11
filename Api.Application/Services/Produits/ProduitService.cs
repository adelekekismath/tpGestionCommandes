namespace Api.Application.Services.Produits;

using Api.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using Api.ViewModel.DTOs;
using Api.Domain.Entities;
public class ProduitService( AppDbContext _db ) : IProduitService
{
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

        _db.Produits.Add(produit);
        await _db.SaveChangesAsync();
        return produit;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var produit = await _db.Produits.FindAsync(id);
        if (produit == null) return false;

        _db.Produits.Remove(produit);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Produit>> GetAllAsync()
    {
        return await _db.Produits.ToListAsync();
    }

    public async Task<Produit?> GetByIdAsync(int id)
    {
        return await _db.Produits.FindAsync(id);
    }

    public async Task<bool> UpdateAsync(int id, ProduitBaseDto dto)
    {
        var produit = await _db.Produits.FindAsync(id);
        if (produit == null) return false;

        produit.Nom = dto.Nom;
        produit.Description = dto.Description;
        produit.Prix = dto.Prix;
        produit.Stock = dto.Stock;
        produit.CategorieId = dto.CategorieId;

        _db.Produits.Update(produit);
        await _db.SaveChangesAsync();
        return true;
    }
}