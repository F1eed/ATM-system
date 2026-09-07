using ConsoleClient.Application.Contracts.Models;

namespace ConsoleClient.Application.Contracts.Operations;

public static class Deposit
{
    public readonly record struct Request(Guid SessionId, decimal Amount);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(AccountDto AccountDto) : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }
}