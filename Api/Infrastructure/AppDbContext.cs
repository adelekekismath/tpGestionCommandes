using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Commande> Commandes => Set<Commande>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>().
        HasMany(c => c.Commandes)
        .WithOne(c => c.Client)
        .HasForeignKey(c => c.ClientId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Commande>()
        .Property(c => c.MontantTotal)
        .HasPrecision(18, 2);

        modelBuilder.Entity<Commande>()
        .ToTable(t => t.HasCheckConstraint(
            "CK_Commande_Statut_Valid",
            "STATUT IN ('EnAttente', 'EnCours', 'Livrée', 'Annulée', 'Expédiée')"

        ));

      

        base.OnModelCreating(modelBuilder);
    }


}