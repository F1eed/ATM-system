using Service.Application.Contracts.Operation.Models;

namespace Service.Application.Contracts.Account.Operations;

public static class CheckHistory
{
    public readonly record struct PageToken(long Key);

    public readonly record struct Request(
        Guid SessionId,
        PageToken? PageToken,
        int PageSize);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(
            IReadOnlyCollection<BaseOperationDto> Operations,
            PageToken? PageToken) : Response;

        public sealed record NotUserSession : Response;

        public sealed record Failure(string Message) : Response;
    }
}