namespace ConsoleClient.Application.Contracts.Operations;

public static class CreateAccount
{
    public readonly record struct Request(
        Guid SessionId,
        IReadOnlyCollection<AccountEntry> Entries);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(IReadOnlyCollection<string> CardNumbers) : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }

    public record AccountEntry(string CardNumber, string PinCode);
}