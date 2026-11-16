using Service.Application.Contracts.Account.Models;
using Service.Application.Contracts.Account.Operations;
using Service.Application.Model.Accounts;

namespace Service.Application.Mapping;

public static class AccountMappingExtensions
{
    public static AccountDto MapToDto(this Account account)
    {
        return new AccountDto(account.CardNumber, account.Balance.Value);
    }

    public static Account MapToDomain(this CreateAccount.AccountEntry accountEntry)
    {
        return Account.CreateNew(AccountId.Default, accountEntry.CardNumber, accountEntry.PinCode);
    }
}