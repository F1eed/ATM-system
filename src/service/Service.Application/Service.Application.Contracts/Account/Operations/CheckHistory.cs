using Service.Application.Contracts.Operation.Models;

namespace Service.Application.Contracts.Account.Operations;

public static class CheckHistory
{
    public readonly record struct Request(Guid SessionId);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(IReadOnlyCollection<OperationDto> Operations) : Response;

        public sealed record NotUserSession() : Response;

        public sealed record Failure(string Message) : Response;
    }
}