namespace Api.Databases.EntityConfigurations;

using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CommandeConfiguration : IEntityTypeConfiguration<Commande>
{
    public void Configure(EntityTypeBuilder<Commande> builder)
    {
        builder.Property(c => c.MontantTotal)
        .HasPrecision(18, 2);

        builder.HasMany(c => c.LignesCommande)
        .WithOne(c => c.Commande)
        .HasForeignKey(c => c.CommandeId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Commande_Statut_Valid",
            "STATUT IN ('EnAttente', 'EnCours', 'Livrée', 'Annulée', 'Expédiée')"

        ));
    }
}