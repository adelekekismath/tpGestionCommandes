namespace Api.Application.Services.Categories;

using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class CategorieService(AppDbContext _db): ICategorieService
{
    private readonly AppDbContext _context = _db;

    public async Task<IEnumerable<Categorie>> GetAllAsync()
    {
        return await _context.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Categorie?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Categorie> CreateAsync(CategorieBaseDto dto)
    {
        var categorie = new Categorie
        {
            Nom = dto.Nom,
            Description = dto.Description
        };

        _context.Categories.Add(categorie);
        await _context.SaveChangesAsync();
        return categorie;
    }

    public async Task<bool> UpdateAsync(int id, CategorieBaseDto dto)
    {
        var categorie = await _context.Categories.FindAsync(id);
        if (categorie is null) return false;

        categorie.Nom = dto.Nom;
        categorie.Description = dto.Description;

        _context.Categories.Update(categorie);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var categorie = await _context.Categories.FindAsync(id);
        if (categorie is null) return false;

        _context.Categories.Remove(categorie);
        await _context.SaveChangesAsync();
        return true;
    }
}