using Service.Application.Model.Accounts;

namespace Service.Application.Model.Operations;

public sealed record WithdrawOperation(
    OccuredAt OccuredAt,
    OperationId OperationId,
    AccountId AccountId,
    Money Amount) : IOperation;