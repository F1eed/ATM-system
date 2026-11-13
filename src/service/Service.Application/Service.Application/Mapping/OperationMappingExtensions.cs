using Service.Application.Contracts.Operation.Models;
using Service.Application.Model.Operations;
using System.Diagnostics;

namespace Service.Application.Mapping;

public static class OperationMappingExtensions
{
    public static BaseOperationDto MapToDto(this IOperation operation)
    {
        return operation switch
        {
            CheckBalanceOperation checkBalanceOperation =>
                checkBalanceOperation.MapToDto(),
            WithdrawOperation withdrawOperation =>
                withdrawOperation.MapToDto(),
            DepositOperation depositOperation =>
                depositOperation.MapToDto(),
            _ => throw new UnreachableException(
                $"mapping for type {operation.GetType().Name} is not implementing"),
        };
    }

    private static CheckBalanceOperationDto MapToDto(this CheckBalanceOperation operation)
    {
        return new CheckBalanceOperationDto(
            operation.OperationId.Value,
            operation.AccountId.Value,
            operation.OccuredAt.Value,
            operation.Balance.Value);
    }

    private static WithdrawOperationDto MapToDto(this WithdrawOperation operation)
    {
        return new WithdrawOperationDto(
            operation.OperationId.Value,
            operation.AccountId.Value,
            operation.OccuredAt.Value,
            operation.Amount.Value);
    }

    private static DepositOperationDto MapToDto(this DepositOperation operation)
    {
        return new DepositOperationDto(
            operation.OperationId.Value,
            operation.AccountId.Value,
            operation.OccuredAt.Value,
            operation.Amount.Value);
    }
}