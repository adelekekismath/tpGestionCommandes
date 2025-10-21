using System.ComponentModel.DataAnnotations;
using Api.Domain.Enums;

namespace Api.Contracts;

public record CommandeCreateDto(
    [Required, StringLength(32)] string NumeroCommande,
    [Required, Range(0.01, double.MaxValue)] decimal MontantTotal,
    [Required, StringLength(30)] StatutCommande Statut,
    [Required] int ClientId
);

public record CommandeUpdateDto(
    [Required, Range(0.01, double.MaxValue)] decimal MontantTotal,
    [Required] StatutCommande Statut
);
