using Lab1.Application.Contracts.Session.Models;

namespace Lab1.Application.Contracts.Session.Operations;

public static class CreateUserSession
{
    public readonly record struct Request(string Number, string PinCode);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(SessionDto Session) : Response;

        public sealed record Failure(string Error) : Response;
    }
}