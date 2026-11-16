namespace ConsoleClient.Application.Abstractions.Account.Operations;

public static class ServiceCheckBalance
{
    public readonly record struct Request(Guid SessionId);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(decimal Balance) : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }
}