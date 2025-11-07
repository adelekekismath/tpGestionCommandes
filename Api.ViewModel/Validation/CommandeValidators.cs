using Api.ViewModel.DTOs;
using FluentValidation;
using Api.Domain.Enums;

namespace Api.ViewModel.Validation;
public class CommandeCreateDtoValidator : AbstractValidator<CommandeCreateDto>
{
    

    public CommandeCreateDtoValidator()
    {
        RuleFor(x => x.NumeroCommande)
            .NotEmpty().WithMessage("Le numéro de commande est requis.")
            .MaximumLength(32).WithMessage("Le numéro de commande ne peut pas dépasser 32 caractères.");

        RuleFor(x => x.MontantTotal)
            .GreaterThan(0).WithMessage("Le montant total doit être supérieur à 0.");

        RuleFor(x => x.Statut)
            .NotEmpty().WithMessage("Le statut est requis.")
            .Must(s => StatutCommandeHelper.StatutsValides.Contains(s))
            .WithMessage("Le statut doit être l'une des valeurs suivantes : EnAttente, EnCours, Livrée ou Annulée.");

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("L'identifiant du client doit être supérieur à 0.");
    }
}

public class CommandeUpdateDtoValidator : AbstractValidator<CommandeUpdateDto>
{
    public CommandeUpdateDtoValidator()
    {
        RuleFor(x => x.MontantTotal)
            .GreaterThan(0).WithMessage("Le montant total doit être supérieur à 0.");

        RuleFor(x => x.Statut).Must(s => StatutCommandeHelper.StatutsValides.Contains(s))
            .WithMessage("Le statut doit être l'une des valeurs suivantes : EnAttente, EnCours, Livrée, Annulée ou Expédiée.");
    }
}
