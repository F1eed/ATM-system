using ConsoleClient.Application.Contracts.Models;

namespace ConsoleClient.Application.Contracts.Operations;

public static class CheckHistory
{
    public readonly record struct Request(Guid SessionId);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(IReadOnlyCollection<OperationDto> OperationDtos) : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }
}