using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;

public record ClientCreateDto(
    [Required, StringLength(100)] string Nom,
    [Required, StringLength(100)] string Prenom,
    [Required, EmailAddress] string Email,
    [Required, StringLength(30)] string Telephone,
    [Required, StringLength(200)] string Adresse
);

public record ClientUpdateDto(
    [Required, StringLength(100)] string Nom,
    [Required, StringLength(100)] string Prenom,
    [Required, EmailAddress] string Email,
    [Required, StringLength(30)] string Telephone,
    [Required, StringLength(200)] string Adresse
);