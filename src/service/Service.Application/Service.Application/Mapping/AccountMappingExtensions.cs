using Service.Application.Contracts.Account.Models;
using Service.Application.Model.Accounts;

namespace Service.Application.Mapping;

public static class AccountMappingExtensions
{
    public static AccountDto MapToDto(this Account account)
        => new AccountDto(account.CardNumber, account.Balance);
}