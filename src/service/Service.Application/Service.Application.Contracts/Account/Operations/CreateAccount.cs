using Service.Application.Contracts.Account.Models;

namespace Service.Application.Contracts.Account.Operations;

public static class CreateAccount
{
    public readonly record struct Request(Guid SessionId, IReadOnlyCollection<AccountEntry> Entries);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(IReadOnlyCollection<AccountDto> Accounts) : Response;

        public sealed record Failure(string Error) : Response;
    }

    public class AccountEntry // Траблы с неймингом
    {
        public AccountEntry(string cardNumber, string pinCode)
        {
            CardNumber = cardNumber;
            PinCode = pinCode;
        }

        public string CardNumber { get; }

        public string PinCode { get; }
    }
}