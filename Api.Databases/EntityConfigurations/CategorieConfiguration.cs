namespace Api.Databases.EntityConfigurations;

using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class CategorieConfiguration : IEntityTypeConfigurationuration<Categorie>
{
    public Configure(EntityTypeBuilder<Categorie> builder)
    {
        modelBuilder.Entity<Categorie>()
        .HasMany(c => c.Produits)
        .WithOne(c => c.Categorie)
        .HasForeignKey(c => c.CategorieId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}