using Api.Contracts;
using FluentValidation;

namespace Api.Validation;

public class CommandeCreateDtoValidator : AbstractValidator<CommandeCreateDto>
{
    public CommandeCreateDtoValidator()
    {
        RuleFor(x => x.NumeroCommande).NotEmpty().MaximumLength(32);
        RuleFor(x => x.MontantTotal).GreaterThan(0);
        RuleFor(x => x.Statut).IsInEnum();
        RuleFor(x => x.ClientId).GreaterThan(0);
    }
}

public class CommandeUpdateDtoValidator : AbstractValidator<CommandeUpdateDto>
{
    public CommandeUpdateDtoValidator()
    {
        RuleFor(x => x.MontantTotal).GreaterThan(0);
        RuleFor(x => x.Statut).IsInEnum();
    }
}
