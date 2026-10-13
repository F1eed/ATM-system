using Service.Application.Contracts.Account.Models;

namespace Service.Application.Contracts.Account.Operations;

public static class CreateAccount
{
    public readonly record struct Request(Guid SessionId, IReadOnlyCollection<AccountEntry> Accounts);

    public record AccountEntry(string CardNumber, string PinCode);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(IReadOnlyCollection<AccountDto> Accounts) : Response;

        public sealed record Failure(string Error) : Response;
    }
}
