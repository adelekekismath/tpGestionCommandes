namespace Api.Databases.EntityConfigurations;

using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasMany(c => c.Commandes)
        .WithOne(c => c.Client)
        .HasForeignKey(c => c.ClientId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}