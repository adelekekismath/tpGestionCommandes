using System.ComponentModel.DataAnnotations;
using Api.Domain.Enums;

namespace Api.ViewModel.DTOs;

public record CommandeCreateDto(
    [Required, StringLength(32)] string NumeroCommande,
    [Required, Range(0.01, double.MaxValue)] decimal MontantTotal,
    [Required, StringLength(30)] string Statut,
    [Required] int ClientId
);



public record CommandeUpdateDto(
    [Required, Range(0.01, double.MaxValue)] decimal MontantTotal,
    [Required] string Statut
);
