using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Lab1.Presentation.Http.Models;

public sealed class CreateAccountRequest
{
    [NotNull]
    [Required]
    public string? PinCode { get; set; }

    [NotNull]
    [Required]
    public string? Number { get; set; }

    [Required]
    public Guid SessionId { get; set; }
}