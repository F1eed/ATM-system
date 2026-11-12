using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Service.Presentation.Http.Models;

public sealed class CreateAccountRequest
{
    [ValidateEnumeratedItems]
    public required IReadOnlyCollection<CreateAccountEntry> Entries { get; init; }

    public required Guid SessionId { get; init; }

    public class CreateAccountEntry
    {
        [Length(minimumLength: 4, maximumLength: 4)]
        public required string PinCode { get; init; }

        [Length(minimumLength: 16, maximumLength: 16)]
        public required string CardNumber { get; init; }
    }
}