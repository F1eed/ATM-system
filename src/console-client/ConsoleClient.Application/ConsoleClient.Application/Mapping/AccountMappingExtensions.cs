using ConsoleClient.Application.Contracts.Models;
using ConsoleClient.Application.Model;

namespace ConsoleClient.Application.Mapping;

public static class AccountMappingExtensions
{
    public static AccountDto MapToDto(this Account account)
        => new AccountDto(account.Balance);
}