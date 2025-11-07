namespace Api.Databases.EntityConfigurations;

using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LigneCommandeConfiguration: IEntityTypeConfigurationuration<LigneCommande>{
    public Config(EntityTypeBuilder<LigneCommande> builder)
    {
        modelBuilder.Entity<LigneCommande>()
        .Property(c => c.PrixUnitaire)
        .HasPrecision(18, 2);
    }
}
