namespace ConsoleClient.Application.Abstractions.Account.Operations;

public static class CreateAccount
{
    public readonly record struct Request(string Number, string PinCode);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success() : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }
}