using Service.Application.Contracts.Account.Models;

namespace Service.Application.Contracts.Account.Operations;

public static class Withdraw
{
    public readonly record struct Request(Guid SessionId, decimal Amount);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(AccountDto Account) : Response;

        public sealed record NotUserSession : Response;

        public sealed record Failure(string Message) : Response;
    }
}