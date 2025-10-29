using Microsoft.EntityFrameworkCore;
using Api.Domain.Entities;

namespace Api.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Commande> Commandes => Set<Commande>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .HasMany(c => c.Commandes)
            .WithOne(o => o.Client)
            .HasForeignKey(o => o.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Commande>()
        .Property(c => c.MontantTotal)
        .HasPrecision(18, 2);

         modelBuilder.Entity<Commande>()
        .ToTable(t => t.HasCheckConstraint(
            "CK_Commande_Statut_Valid",
            "Statut IN ('EnAttente', 'EnCours', 'Livrée', 'Annulée', 'Expédiée')"
        ));

        base.OnModelCreating(modelBuilder);
    }
}