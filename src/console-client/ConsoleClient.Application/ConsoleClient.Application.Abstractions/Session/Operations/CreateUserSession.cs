namespace ConsoleClient.Application.Abstractions.Session.Operations;

public static class CreateUserSession
{
    public readonly record struct Request(string CardNumber, string PinCode);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(Model.Session Session) : Result;

        public sealed record Failure(string ErrorMessage) : Result;
    }
}