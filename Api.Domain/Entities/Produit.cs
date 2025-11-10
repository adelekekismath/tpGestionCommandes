namespace Api.Domain.Entities;

public class Produit
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public float Prix { get; set; } 
    public int Stock { get; set; } 


    public int CategorieId { get; set; }
    public Categorie Categorie { get; set; } = default!;
    public List<LigneCommande> LignesCommande { get; set; } = [];

}