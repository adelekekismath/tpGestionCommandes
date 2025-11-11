namespace Api.ViewModel.DTOs;

public record LigneCommandeCreateDto(
    int Quantite,
    float PrixUnitaire,
    int CommandeId,
    int ProduitId
);

public record LigneCommandeUpdateDto(
    int Quantite,
    float PrixUnitaire,
    int ProduitId
);