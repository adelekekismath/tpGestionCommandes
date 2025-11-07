namespace Api.Databases.EntityConfigurations;

using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProduitConfiguration: IEntityTypeConfigurationuration<Produit>
{
    public Configure(EntityTypeBuilder<Produit> builder)
    {
        modelBuilder.Entity<Produit>()
        .Property(c => c.Prix)
        .HasPrecision(18, 2);

        modelBuilder.Entity<Produit>()
        .HasMany(c => c.LignesCommande)
        .WithOne(c => c.Produit);
    }
}