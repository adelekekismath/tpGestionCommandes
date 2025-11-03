using System.Data;
using Api.Contracts;
using FluentValidation;

namespace Api.Validation;

public class UserValidators : AbstractValidator<UserCreateDto>
{
    public UserValidators()
    {
        RuleFor(x => x.Username)
                 .NotEmpty().WithMessage("Le nom d'utilisateur est requis.")
                 .MinimumLength(4).WithMessage("Le nom d'utilisateur doit contenir au moins 4 caractères.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Le mot de passe est requis.")
            .MinimumLength(6).WithMessage("Le mot de passe doit contenir au moins 6 caractères.")
            .Matches("[A-Z]").WithMessage("Le mot de passe doit contenir au moins une lettre majuscule.")
            .Matches("[a-z]").WithMessage("Le mot de passe doit contenir au moins une lettre minuscule.")
            .Matches("[0-9]").WithMessage("Le mot de passe doit contenir au moins un chiffre.");
    }

}