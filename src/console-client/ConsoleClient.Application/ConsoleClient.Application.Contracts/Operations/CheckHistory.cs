using ConsoleClient.Application.Contracts.Models.Operations;

namespace ConsoleClient.Application.Contracts.Operations;

public static class CheckHistory
{
    public readonly record struct Request(
        Guid SessionId,
        int OperationsCount);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(IReadOnlyCollection<BaseOperationDto> OperationDtos) : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }
}