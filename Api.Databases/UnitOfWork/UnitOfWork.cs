using Api.Databases.Repositories.BaseRepository;
using Api.Databases.Contexts;
using Api.Domain.Entities;

namespace Api.Databases.UnitOfWork;

public class UnitOfWork : IUnitOfWork{

    private readonly AppDbContext _context;

    public IBaseRepository<Client> Clients { get; private set;}
    public IBaseRepository<Categorie> Categories { get; private set; }
    public IBaseRepository<Commande> Commandes{ get; private set;}
    public IBaseRepository<Produit> Produits{ get; private set;}
    public IBaseRepository<LigneCommande> LigneCommandes{ get; private set;}

    public UnitOfWork(AppDbContext context){
        _context = context;
        Categories = new BaseRepository<Categorie>(_context);
        Commandes = new BaseRepository<Commande>(_context);
        Produits = new BaseRepository<Produit>(_context);
        LigneCommandes = new BaseRepository<LigneCommande>(_context);
        Clients = new BaseRepository<Client>(_context);
    }

    public void Dispose(){
        _context.Dispose();
    }

    public async Task<int> SaveChangesAsync(){
        return await _context.SaveChangesAsync();
    }
}