using Service.Application.Model.Accounts;

namespace Service.Application.Model.Operations;

public sealed record CheckBalanceOperation(
    OccuredAt OccuredAt,
    OperationId OperationId,
    AccountId AccountId,
    Money RemainingBalance) : IOperation;