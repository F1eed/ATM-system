using Lab1.Application.Contracts.Account.Models;
using Lab1.Application.Models.Accounts;

namespace Lab1.Application.Mapping;

public static class AccountMappingExtensions
{
    public static AccountDto MapToDto(this Account account)
        => new AccountDto(account.Number, account.Balance);
}