namespace Api.Application.Services.LignesCommandes;

using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using Api.ViewModel.DTOs;
using Api.Domain.Entities;

public class LigneCommandeService(AppDbContext _context): ILigneCommandeService
{
    private readonly AppDbContext _db = _context;

    public async Task<IEnumerable<LigneCommande>> GetAllAsync()
    {
        return await _db.LigneCommandes.AsNoTracking().ToListAsync();
    }

    public async Task<LigneCommande?> GetByIdAsync(int id)
    {
        return await _db.LigneCommandes.FindAsync(id);
    }

    public async Task<LigneCommande> CreateAsync(LigneCommandeCreateDto dto)
    {
        var ligneCommande = new LigneCommande
        {
            CommandeId = dto.CommandeId,
            ProduitId = dto.ProduitId,
            Quantite = dto.Quantite,
            PrixUnitaire = dto.PrixUnitaire
        };

        _db.LigneCommandes.Add(ligneCommande);
        await _db.SaveChangesAsync();
        return ligneCommande;
    }

    public async Task<bool> UpdateAsync(int id, LigneCommandeUpdateDto dto)
    {
        var ligneCommande = await _db.LigneCommandes.FindAsync(id);
        if (ligneCommande is null) return false;

        ligneCommande.ProduitId = dto.ProduitId;
        ligneCommande.Quantite = dto.Quantite;
        ligneCommande.PrixUnitaire = dto.PrixUnitaire;

        _db.LigneCommandes.Update(ligneCommande);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ligneCommande = await _db.LigneCommandes.FindAsync(id);
        if (ligneCommande is null) return false;

        _db.LigneCommandes.Remove(ligneCommande);
        await _db.SaveChangesAsync();
        return true;
    }
}