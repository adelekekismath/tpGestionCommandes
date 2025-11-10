namespace  Api.ViewModel.Validation;
using Api.ViewModel.DTOs;
using FluentValidation; 

public class ProduitBaseValidator : AbstractValidator<ProduitBaseDto>
{
    public ProduitBaseValidator()
    {
        RuleFor(x => x.Nom)
            .NotEmpty().WithMessage("Le nom du produit est requis.")
            .MaximumLength(100).WithMessage("Le nom du produit ne doit pas dépasser 100 caractères.");
        
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La description du produit ne doit pas dépasser 500 caractères.");
        
        RuleFor(x => x.Prix)
            .GreaterThan(0).WithMessage("Le prix doit être supérieur à zéro.");
        
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Le stock ne peut pas être négatif.");
        
        RuleFor(x => x.CategorieId)
            .NotEmpty().WithMessage("L'ID de la catégorie est requis.");
    }
}