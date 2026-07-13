using Lab1.Application.Contracts.Account.Models;

namespace Lab1.Application.Contracts.Account.Operations;

public static class Deposit
{
    public readonly record struct Request(Guid SessionId, decimal Amount);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(AccountDto Account) : Response;

        public sealed record NotUserSession : Response;

        public sealed record Failure(string Error) : Response;
    }
}