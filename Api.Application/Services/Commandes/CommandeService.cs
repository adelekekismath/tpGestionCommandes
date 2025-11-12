namespace Api.Application.Services.Commandes;

using Api.Databases.Contexts;
using Api.Domain.Entities;
using Api.ViewModel.DTOs;
using Microsoft.EntityFrameworkCore;

using Api.Databases.UnitOfWork;

public class CommandeService(IUnitOfWork unitOfWork) : ICommandeService
{
    private readonly IUnitOfWork _unityOfWork = unitOfWork ;

    public async Task<IEnumerable<Commande>> GetAllAsync()
        => await _unityOfWork.Commandes.GetAllAsync();

    public async Task<Commande?> GetByIdAsync(int id)
    {
        return await _unityOfWork.Commandes.GetByIdAsync(id);
    }

    public async Task<Commande> CreateAsync(CommandeCreateDto dto)
    {
        var entity = new Commande
        {
            NumeroCommande = dto.NumeroCommande,
            MontantTotal = dto.MontantTotal,
            Statut = dto.Statut,
            ClientId = dto.ClientId
        };

        _unityOfWork.Commandes.AddAsync(entity);
        await _unityOfWork.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, CommandeUpdateDto dto)
    {
        var commande = await _unityOfWork.Commandes.GetByIdAsync(id);
        if (commande is null) return false;

        commande.MontantTotal = dto.MontantTotal;
        commande.Statut = dto.Statut;
        _unityOfWork.Commandes.UpdateAsync(commande);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var commande = await _unityOfWork.Commandes.GetByIdAsync(id);
        if (commande is null) return false;

        _unityOfWork.Commandes.DeleteAsync(commande);
        await _unityOfWork.SaveChangesAsync();
        return true;
    }
}
