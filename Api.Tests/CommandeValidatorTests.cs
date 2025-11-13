using Api.ViewModel.DTOs;
using Api.ViewModel.Validation;
using Api.Domain.Enums;
using FluentAssertions;

namespace Api.Tests;

public class CommandeValidatorTests
{
    [Fact]
    public void CreateDto_Invalid_When_Montant_Negatif()
    {
        var dto = new CommandeCreateDto("ABC123", -10, "EnAttente", 1);
        var validator = new CommandeCreateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CreateDto_Invalid_When_Statut_Invalide()
    {
        var dto = new CommandeCreateDto("ABC123", 10, "999", 1);
        var validator = new CommandeCreateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UpdateDto_Invalid_When_Montant_Negatif()
    {
        var dto = new CommandeUpdateDto(-20, "EnAttente");
        var validator = new CommandeUpdateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UpdateDto_Invalid_When_Statut_Invalide()
    {
        var dto = new CommandeUpdateDto(20, "999");
        var validator = new CommandeUpdateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Statut");
    }

    [Fact]
    public void CreateDto_Valid_When_All_Fields_Correct()
    {
        var dto = new CommandeCreateDto("XYZ789", 50, "EnCours", 2);
        var validator = new CommandeCreateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void UpdateDto_Valid_When_All_Fields_Correct()
    {
        var dto = new CommandeUpdateDto(100, "Expédiée");
        var validator = new CommandeUpdateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

   
}
