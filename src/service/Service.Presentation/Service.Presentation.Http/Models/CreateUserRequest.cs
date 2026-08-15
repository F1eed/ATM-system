using Microsoft.Extensions.Options;

namespace Service.Presentation.Http.Models;

public class CreateUserRequest
{
    [ValidateEnumeratedItems]
    public required IReadOnlyCollection<UserEntry> Entries { get; init; }

    public class UserEntry
    {
        public required string Number { get; init; }

        public required string PinCode { get; init; }
    }
}