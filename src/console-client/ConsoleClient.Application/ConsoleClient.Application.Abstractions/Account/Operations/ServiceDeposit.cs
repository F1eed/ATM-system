namespace ConsoleClient.Application.Abstractions.Account.Operations;

public static class ServiceDeposit
{
    public readonly record struct Request(Guid SessionId, decimal Amount);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(Model.Account Account) : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }
}