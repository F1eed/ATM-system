using Service.Application.Contracts.Session.Models;

namespace Service.Application.Contracts.Session.Operations;

public static class CreateUserSession
{
    public readonly record struct Request(IReadOnlyCollection<UserEntry> Entries);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(IReadOnlyCollection<SessionDto> Sessions) : Response;

        public sealed record Failure(string Error) : Response;
    }

    public class UserEntry
    {
        public UserEntry(string number, string pinCode)
        {
            Number = number;
            PinCode = pinCode;
        }

        public string Number { get; set; }

        public string PinCode { get; set; }
    }
}