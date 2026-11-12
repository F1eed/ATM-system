using System.ComponentModel.DataAnnotations;

namespace ConsoleClient.Infrastructure.AccountService.Options;

public sealed class AccountClientOptions
{
    [Required]
    public required Uri Address { get; set; }

    [Range(1, 10)]
    public int PageSize { get; init; }
}