using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Entities;

public class User
{
    public int Id { get; set; }
    [Key]
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}
