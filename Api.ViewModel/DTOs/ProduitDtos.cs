namespace Api.ViewModel.DTOs;

public record ProduitBaseDto
(
    string Nom,
    string Description,
    float Prix,
    int Stock,
    int CategorieId
);