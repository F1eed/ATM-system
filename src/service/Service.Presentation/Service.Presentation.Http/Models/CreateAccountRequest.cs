using Microsoft.Extensions.Options;

namespace Service.Presentation.Http.Models;

public sealed class CreateAccountRequest
{
    [ValidateEnumeratedItems]
    public required IReadOnlyCollection<AccountEntryRequest> Entries { get; init; }

    public required Guid SessionId { get; init; }

    public class AccountEntryRequest
    {
        public required string PinCode { get; init; }

        public required string CardNumber { get; init; }
    }
}