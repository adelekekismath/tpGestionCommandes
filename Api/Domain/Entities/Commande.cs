namespace Api.Domain.Entities;

public class Commande
{
    public int Id { get; set; }
    public string NumeroCommande { get; set; } = default!;
    public DateTime DateCommande { get; set; } = DateTime.UtcNow;
    public decimal MontantTotal { get; set; }
    public string Statut { get; set; } = "EnAttente";

   
    public int ClientId { get; set; }
    public Client Client { get; set; } = default!;
}