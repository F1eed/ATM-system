using Service.Application.Contracts.Operation.Models;

namespace Service.Presentation.Http.Models;

public sealed record CheckHistoryResponse(IReadOnlyCollection<OperationDto> Operations);
