namespace Api.ViewModel.Validation;
using Api.ViewModel.DTOs;
using FluentValidation;

public class CategorieBaseValidator : AbstractValidator<CategorieBaseDto>
{

    public CategorieBaseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom de la catégorie est requis.")
            .MaximumLength(100).WithMessage("Le nom de la catégorie ne doit pas dépasser 100 caractères.");
    }
}
