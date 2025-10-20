namespace Api.Domain.Entities;

public class Client
{
    public int Id { get; set; }
    public string Nom { get; set; } = default!;
    public string Prenom { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Telephone { get; set; } = default!;
    public string Adresse { get; set; } = default!;
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Commande> Commandes { get; set; } = new List<Commande>();
}