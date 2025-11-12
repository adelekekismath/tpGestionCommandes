namespace Api.Application.Services.LignesCommandes;

using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.UnitOfWork;

public class LigneCommandeService(IUnitOfWork unitOfWork): ILigneCommandeService
{
    private readonly IUnitOfWork _unityOfWork = unitOfWork;

    public async Task<IEnumerable<LigneCommande>> GetAllAsync()
    {
        return await _unityOfWork.LigneCommandes.GetAllAsync();
    }

    public async Task<LigneCommande?> GetByIdAsync(int id)
    {
        return await _unityOfWork.LigneCommandes.GetByIdAsync(id);
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

        _unityOfWork.LigneCommandes.AddAsync(ligneCommande);
        await _unityOfWork.SaveChangesAsync();
        return ligneCommande;
    }

    public async Task<bool> UpdateAsync(int id, LigneCommandeUpdateDto dto)
    {
        var ligneCommande = await _unityOfWork.LigneCommandes.GetByIdAsync(id);
        if (ligneCommande is null) return false;

        ligneCommande.ProduitId = dto.ProduitId;
        ligneCommande.Quantite = dto.Quantite;
        ligneCommande.PrixUnitaire = dto.PrixUnitaire;

        _unityOfWork.LigneCommandes.UpdateAsync(ligneCommande);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ligneCommande = await _unityOfWork.LigneCommandes.GetByIdAsync(id);
        if (ligneCommande is null) return false;

        _unityOfWork.LigneCommandes.DeleteAsync(ligneCommande);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }
}