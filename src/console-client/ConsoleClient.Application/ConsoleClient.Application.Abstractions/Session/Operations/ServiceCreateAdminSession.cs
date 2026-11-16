namespace ConsoleClient.Application.Abstractions.Session.Operations;

public static class ServiceCreateAdminSession
{
    public readonly record struct Request(string Password);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(Model.Session Session) : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }
}