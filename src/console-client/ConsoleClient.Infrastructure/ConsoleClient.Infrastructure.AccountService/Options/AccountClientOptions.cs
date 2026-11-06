using System.ComponentModel.DataAnnotations;

namespace ConsoleClient.Infrastructure.AccountService.Options;

public sealed class AccountClientOptions
{
    [MinLength(1)]
    public required Uri Address { get; set; }
}