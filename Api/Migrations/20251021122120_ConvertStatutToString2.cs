using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class ConvertStatutToString2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Commande_Statut_Valid",
                table: "Commandes",
                sql: "Statut IN ('EnAttente', 'EnCours', 'Livrée', 'Annulée', 'Expédiée')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Commande_Statut_Valid",
                table: "Commandes");
        }
    }
}
