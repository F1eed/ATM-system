using ConsoleClient.Application.Contracts.Models;

namespace ConsoleClient.Application.Contracts.Operations;

public static class CreateUserSession
{
    public readonly record struct Request(string CardNumber, string PinCode);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(SessionDto Session) : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }
}