using ConsoleClient.Application.Contracts.Models.Operations;
using ConsoleClient.Application.Model.Operations;
using System.Diagnostics;

namespace ConsoleClient.Application.Mapping;

public static class OperationsMappingExtensions
{
    public static BaseOperationDto MapToDto(this IOperation operation)
    {
        return operation switch
        {
            CheckBalanceOperation checkBalance => checkBalance.MapToDto(),
            DepositOperation deposit => deposit.MapToDto(),
            WithdrawOperation withdraw => withdraw.MapToDto(),
            _ => throw new UnreachableException(),
        };
    }

    private static CheckBalanceOperationDto MapToDto(
        this CheckBalanceOperation operation)
    {
        return new CheckBalanceOperationDto(operation.OccuredAt, operation.Balance);
    }

    private static DepositOperationDto MapToDto(
        this DepositOperation operation)
    {
        return new DepositOperationDto(operation.OccuredAt, operation.Amount);
    }

    private static WithdrawOperationDto MapToDto(
        this WithdrawOperation operation)
    {
        return new WithdrawOperationDto(operation.OccuredAt, operation.Amount);
    }
}