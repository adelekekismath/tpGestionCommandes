using Api.Contracts;
using Api.Validation;
using FluentAssertions;

namespace Api.Tests;

public class CommandeValidatorTests
{
    [Facts]
    public void CreateDto_Invalid_When_Montant_Negatif()
    {
        var dto = new CommandeCreateDto("ABC123", -10, StatutCommande.EnAttente, 1);
        var validator = new CommandeCreateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CreateDto_Invalid_When_Statut_Invalide()
    {
        var dto = new CommandeCreateDto("ABC123", 10, (StatutCommande)999, 1);
        var validator = new CommandeCreateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UpdateDto_Invalid_When_Montant_Negatif()
    {
        var dto = new CommandeUpdateDto(-20, StatutCommande.Traitee);
        var validator = new CommandeUpdateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UpdateDto_Invalid_When_Statut_Invalide()
    {
        var dto = new CommandeUpdateDto(20, (StatutCommande)999);
        var validator = new CommandeUpdateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Statut");
    }

    [Fact]
    public void CreateDto_Valid_When_All_Fields_Correct()
    {
        var dto = new CommandeCreateDto("XYZ789", 50, StatutCommande.Traitee, 2);
        var validator = new CommandeCreateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void UpdateDto_Valid_When_All_Fields_Correct()
    {
        var dto = new CommandeUpdateDto(100, StatutCommande.Expediee);
        var validator = new CommandeUpdateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

   
}
