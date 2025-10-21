using Api.Contracts;
using FluentValidation;

namespace Api.Validation;

public class ClientCreateDtoValidator : AbstractValidator<ClientCreateDto>
{
    public ClientCreateDtoValidator()
    {
        RuleFor(x => x.Nom)
            .NotEmpty().WithMessage("Le nom est requis.")
            .MinimumLength(2).WithMessage("Le nom doit contenir au moins 2 caractères.");

        RuleFor(x => x.Prenom)
            .NotEmpty().WithMessage("Le prénom est requis.")
            .MinimumLength(2).WithMessage("Le prénom doit contenir au moins 2 caractères.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("L'email est requis.")
            .EmailAddress().WithMessage("L'email est invalide.");

        RuleFor(x => x.Telephone)
            .NotEmpty().WithMessage("Le téléphone est requis.")
            .Matches(@"^\d{10}$").WithMessage("Le téléphone doit contenir 10 chiffres.");

        RuleFor(x => x.Adresse)
            .NotEmpty().WithMessage("La ville est requise.");
    }
}

public class ClientUpdateDtoValidator : AbstractValidator<ClientUpdateDto>
{
    public ClientUpdateDtoValidator()
    {
        RuleFor(x => x.Nom)
            .NotEmpty().WithMessage("Le nom est requis.")
            .MinimumLength(2).WithMessage("Le nom doit contenir au moins 2 caractères.");

        RuleFor(x => x.Prenom)
            .NotEmpty().WithMessage("Le prénom est requis.")
            .MinimumLength(2).WithMessage("Le prénom doit contenir au moins 2 caractères.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("L'email est requis.")
            .EmailAddress().WithMessage("L'email est invalide.");

        RuleFor(x => x.Telephone)
            .NotEmpty().WithMessage("Le téléphone est requis.")
            .Matches(@"^\d{10}$").WithMessage("Le téléphone doit contenir 10 chiffres.");

        RuleFor(x => x.Adresse)
            .NotEmpty().WithMessage("La ville est requise.");
    }
}
