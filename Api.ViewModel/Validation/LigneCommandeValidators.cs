namespace Api.ViewModel.Validation;
using Api.ViewModel.DTOs;
using FluentValidation;

public class LigneCommandeCreateValidator : AbstractValidator<LigneCommandeCreateDto>
{
    public LigneCommandeCreateValidator()
    {
        RuleFor(x => x.Quantite)
            .GreaterThan(0).WithMessage("La quantité doit être supérieure à zéro.");
        RuleFor(x => x.PrixUnitaire)
            .GreaterThan(0).WithMessage("Le prix unitaire doit être supérieur à zéro.");
        RuleFor(x => x.CommandeId)
            .NotEmpty().WithMessage("L'ID de la commande est requis.");
        RuleFor(x => x.ProduitId)
            .NotEmpty().WithMessage("L'ID du produit est requis.");
    }

}

public class LigneCommandeUpdateValidator: AbstractValidator<LigneCommandeUpdateDto>
{
    public LigneCommandeUpdateValidator()
    {
        RuleFor(x => x.Quantite)
            .GreaterThan(0).WithMessage("La quantité doit être supérieure à zéro.");
        RuleFor(x => x.PrixUnitaire)
            .GreaterThan(0).WithMessage("Le prix unitaire doit être supérieur à zéro.");
        RuleFor(x => x.ProduitId)
            .NotEmpty().WithMessage("L'ID du produit est requis.");
    }
}
