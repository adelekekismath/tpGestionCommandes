namespace Api.Domain.Enums;

public enum StatutCommande
{
    EnAttente,
    Traitee,
    Expediee,
    Livree,
    Annulee
}

public static class StatutCommandeHelper
{
    public static string[] StatutsValides { get; } =
    [
        "EnAttente",
        "EnCours",
        "Livrée",
        "Annulée",
        "Expédiée"
    ];
}