using System.ComponentModel.DataAnnotations;

namespace Lab1.Presentation.Http.Models;

public class WithdrawRequest
{
    [Required]
    public Guid SessionId { get; set; }

    [Required]
    public decimal Amount { get; set; }
}