namespace Api.Domain.Entities;

public class Categorie
{
    public int Id;
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<Produit> Produits { get; set; } = [];
}