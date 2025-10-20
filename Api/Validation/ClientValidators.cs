using Api.Contracts;
using FluentValidation;

namespace Api.Validation;

public class ClientCreateDtoValidator : AbstractValidator<ClientCreateDto>
{
    public ClientCreateDtoValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Prenom).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Telephone).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Adresse).NotEmpty().MaximumLength(200);
    }
}

public class ClientUpdateDtoValidator : AbstractValidator<ClientUpdateDto>
{
    public ClientUpdateDtoValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Prenom).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Telephone).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Adresse).NotEmpty().MaximumLength(200);
    }
}
