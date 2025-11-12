using Api.Databases.Repositories.BaseRepository;
using Api.Domain.Entities;

namespace Api.Databases.UnitOfWork;


public interface IUnitOfWork: IDisposable{
    IBaseRepository<Client> Clients { get; }
    IBaseRepository<Categorie> Categories { get; }
    IBaseRepository<Commande> Commandes{ get; }
    IBaseRepository<Produit> Produits{ get; }
    IBaseRepository<LigneCommande> LigneCommandes{ get; }
    Task<int> SaveChangesAsync();
}