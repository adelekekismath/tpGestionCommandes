namespace Api.Databases.EntityConfigurations;

using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProduitConfiguration: IEntityTypeConfiguration<Produit>
{
    public void Configure(EntityTypeBuilder<Produit> builder)
    {
        builder
            .HasKey(c => c.Id);

        builder
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Prix).HasColumnType("decimal(18,2)");

        builder
        .HasMany(c => c.LignesCommande)
        .WithOne(c => c.Produit);
    }
}