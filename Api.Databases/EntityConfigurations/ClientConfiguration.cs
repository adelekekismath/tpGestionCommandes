namespace Api.Databases.EntityConfigurations;

using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class ClientConfiguration : IEntityTypeConfigurationuration<Client>
{
    public Configure(EntityTypeBuilder<Client> builder)
    {
        builder.Entity<Client>().
        HasMany(c => c.Commandes)
        .WithOne(c => c.Client)
        .HasForeignKey(c => c.ClientId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}