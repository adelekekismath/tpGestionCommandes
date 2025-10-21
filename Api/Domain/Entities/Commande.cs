namespace Api.Domain.Entities;
using Api.Domain.Enums;

public class Commande
{
    public int Id { get; set; }
    public string NumeroCommande { get; set; } = default!;
    public DateTime DateCommande { get; set; } = DateTime.UtcNow;
    public decimal MontantTotal { get; set; }
    public StatutCommande Statut { get; set; } = StatutCommande.EnAttente;

   
    public int ClientId { get; set; }
    public Client Client { get; set; } = default!;
}