using Api.Contracts;
using Api.Domain.Enums;
using Api.Validation;       
using FluentAssertions;

using Xunit;

namespace Api.Tests;


public class ClientValidatorTests
{
    [Fact]
    public void CreateDto_Invalid_When_Nom_Too_Short()
    {
        var dto = new ClientCreateDto("A", "prenom", "email@example.com", "0600000000", "Adresse");
        var validator = new ClientCreateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CreateDto_Invalid_When_Email_Invalid()
    {
        var dto = new ClientCreateDto("Nom", "Prenom", "invalid-email", "0600000000", "Adresse");
        var validator = new ClientCreateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CreateDto_Valid_When_All_Fields_Correct()
    {
        var dto = new ClientCreateDto("Nom", "Prenom", "email@example.com", "0600000000", "Adresse");
        var validator = new ClientCreateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void UpdateDto_Invalid_When_Nom_Too_Short()
    {
        var dto = new ClientUpdateDto("A", "prenom", "email@example.com", "0600000000", "Adresse");
        var validator = new ClientUpdateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UpdateDto_Invalid_When_Email_Invalid()
    {
        var dto = new ClientUpdateDto("Nom", "Prenom", "invalid-email", "0600000000", "Adresse");
        var validator = new ClientUpdateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UpdateDto_Valid_When_All_Fields_Correct()
    {
        var dto = new ClientUpdateDto("Nom", "Prenom", "email@example.com", "0600000000", "Adresse");
        var validator = new ClientUpdateDtoValidator();
        var result = validator.Validate(dto);
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}