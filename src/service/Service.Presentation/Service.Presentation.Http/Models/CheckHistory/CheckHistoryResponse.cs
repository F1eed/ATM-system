using Service.Application.Contracts.Operation.Models;

namespace Service.Presentation.Http.Models.CheckHistory;

public sealed record CheckHistoryResponse
{
    public required IReadOnlyCollection<BaseOperationDto> Operations { get; init; }

    public required string? PageToken { get; init; }
}