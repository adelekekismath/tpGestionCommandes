namespace Api.ViewModel.DTOs;

public record LigneCommandeCreateDto(
    int Quantite,
    decimal PrixUnitaire,
    int CommandeId,
    int ProduitId
);

public record LigneCommandeUpdateDto(
    int Quantite,
    decimal PrixUnitaire,
    int ProduitId
);