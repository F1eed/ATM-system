using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Lab1.Presentation.Http.Models;

public sealed class CreateAdminRequest
{
    [NotNull]
    [Required]
    public string? Password { get; set; }
}