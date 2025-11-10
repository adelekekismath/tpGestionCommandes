namespace Api.Application.Services.Commandes;

using Api.Databases.Contexts;
using Api.Domain.Entities;
using Api.ViewModel.DTOs;
using Microsoft.EntityFrameworkCore;

public class CommandeService : ICommandeService
{
    private readonly AppDbContext _db;
    public CommandeService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Commande>> GetAllAsync()
        => await _db.Commandes.AsNoTracking().ToListAsync();

    public async Task<Commande?> GetByIdAsync(int id)
    {
        return await _db.Commandes
            .Include(c => c.Client)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
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

        _db.Commandes.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, CommandeUpdateDto dto)
    {
        var commande = await _db.Commandes.FindAsync(id);
        if (commande is null) return false;

        commande.MontantTotal = dto.MontantTotal;
        commande.Statut = dto.Statut;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var commande = await _db.Commandes.FindAsync(id);
        if (commande is null) return false;

        _db.Commandes.Remove(commande);
        await _db.SaveChangesAsync();
        return true;
    }
}
