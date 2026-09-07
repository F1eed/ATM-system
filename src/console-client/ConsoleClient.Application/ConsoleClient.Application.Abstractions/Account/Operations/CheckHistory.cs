using ConsoleClient.Application.Model;

namespace ConsoleClient.Application.Abstractions.Account.Operations;

public class CheckHistory
{
    public readonly record struct Request(Guid SessionId);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(IReadOnlyCollection<Operation> OperationEntries) : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }
}