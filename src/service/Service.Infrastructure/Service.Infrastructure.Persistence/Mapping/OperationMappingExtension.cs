using Service.Application.Model.Operations;
using Service.Infrastructure.Persistence.Models.Operation;

namespace Service.Infrastructure.Persistence.Mapping;

public static class OperationMappingExtension
{
    public static string MapToDao(this OperationType type) => type switch
    {
        OperationType.CheckBalance => "check_balance",
        OperationType.Deposit => "deposit",
        OperationType.Withdraw => "withdraw",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static OperationDao MapToDao(this OperationBase operation)
        => new OperationDao(
            operation.OperationId.Value,
            operation.AccountId.Value,
            operation.Type.MapToDao(),
            operation.Balance);
}