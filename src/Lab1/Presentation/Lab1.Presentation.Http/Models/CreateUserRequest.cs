using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Lab1.Presentation.Http.Models;

public class CreateUserRequest
{
    [NotNull]
    [Required]
    [MinLength(3)]
    public string? Number { get; set; }

    [NotNull]
    [Required]
    [MinLength(4)]
    public string? PinCode { get; set; }
}