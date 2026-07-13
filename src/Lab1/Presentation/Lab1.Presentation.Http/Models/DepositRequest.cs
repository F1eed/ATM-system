using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Lab1.Presentation.Http.Models;

public class DepositRequest
{
    [Required]
    public Guid SessionId { get; set; }

    [Required]
    [NotNull]
    public decimal Amount { get; set; }
}