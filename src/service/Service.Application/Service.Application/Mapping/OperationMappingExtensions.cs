using Service.Application.Contracts.Operation.Models;
using Service.Application.Model.Operations;

namespace Service.Application.Mapping;

public static class OperationMappingExtensions
{
    public static OperationTypeDto MapToDto(this OperationType type) => type switch
    {
        OperationType.CheckBalance => OperationTypeDto.CheckBalance,
        OperationType.Deposit => OperationTypeDto.Deposit,
        OperationType.Withdraw => OperationTypeDto.Withdraw,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public static OperationDto MapToDto(this OperationBase operation)
        => new OperationDto(
            operation.OperationId.Value,
            operation.AccountId.Value,
            operation.Type.MapToDto(),
            operation.Balance);
}