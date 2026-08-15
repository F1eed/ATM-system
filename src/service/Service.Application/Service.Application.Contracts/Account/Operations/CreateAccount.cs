using Service.Application.Contracts.Account.Models;

namespace Service.Application.Contracts.Account.Operations;

public static class CreateAccount
{
    public readonly record struct Request(Guid SessionId, IReadOnlyCollection<AccountEntry> Entries);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(IReadOnlyCollection<AccountDto> Account) : Response;

        public sealed record Failure(string Error) : Response;
    }

    public class AccountEntry // Траблы с неймингом
    {
        public AccountEntry(string number, string pinCode)
        {
            Number = number;
            PinCode = pinCode;
        }

        public string Number { get; }

        public string PinCode { get; }
    }
}