namespace Api.Domain.Entities;

public class Categorie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id;
    public string Nom { get; set; } = string.Empty;

    public List<Produit> Produits { get; set; } = [];
}