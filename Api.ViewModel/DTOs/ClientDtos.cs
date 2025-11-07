using System.ComponentModel.DataAnnotations;

namespace Api.ViewModel.DTOs;

public record ClientBaseDto(
    [Required, StringLength(100)] string Nom,
    [Required, StringLength(100)] string Prenom,
    [Required, EmailAddress] string Email,
    [Required, StringLength(30)] string Telephone,
    [Required, StringLength(200)] string Adresse
);

public record ClientReadDto(
    int Id,
    string Nom,
    string Prenom,
    string Email,
    string Telephone,
    string Adresse
);