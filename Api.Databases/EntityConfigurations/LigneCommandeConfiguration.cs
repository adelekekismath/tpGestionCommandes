namespace Api.Databases.EntityConfigurations;

using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LigneCommandeConfiguration: IEntityTypeConfiguration<LigneCommande>{
    public void Configure(EntityTypeBuilder<LigneCommande> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.PrixUnitaire)
        .HasColumnType("decimal(18,2)");
    }
}
