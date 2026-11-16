using ConsoleClient.Application.Model.Operations;

namespace ConsoleClient.Application.Abstractions.Account.Operations;

public static class ServiceCheckHistory
{
    public readonly record struct Request(
        Guid SessionId,
        int OperationsCount);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(IReadOnlyCollection<IOperation> OperationEntries) : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }
}