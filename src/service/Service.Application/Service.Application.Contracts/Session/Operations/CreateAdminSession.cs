using Service.Application.Contracts.Session.Models;

namespace Service.Application.Contracts.Session.Operations;

public static class CreateAdminSession
{
    public readonly record struct Request(string Password);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(SessionDto Session) : Response;

        public sealed record Failure(string Error) : Response;
    }
}