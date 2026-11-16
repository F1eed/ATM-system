using System.ComponentModel.DataAnnotations;

namespace ConsoleClient.Infrastructure.AccountService.Options;

public sealed class AccountClientOptions
{
    [Required]
    public required Uri Address { get; set; }
}