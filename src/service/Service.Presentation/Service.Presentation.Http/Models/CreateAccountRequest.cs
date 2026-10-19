using Microsoft.Extensions.Options;

namespace Service.Presentation.Http.Models;

public sealed class CreateAccountRequest
{
    [ValidateEnumeratedItems]
    public required IReadOnlyCollection<AccountEntry> Entries { get; init; }

    public required Guid SessionId { get; init; }

    public class AccountEntry
    {
        public required string PinCode { get; init; }

        public required string CardNumber { get; init; }
    }
}