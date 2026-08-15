namespace Service.Application.Contracts.Account.Operations;

public static class CheckBalance
{
    public readonly record struct Request(Guid SessionId);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(decimal Balance) : Response;

        public sealed record NotUserSession : Response;

        public sealed record Failure(string Error) : Response;
    }
}