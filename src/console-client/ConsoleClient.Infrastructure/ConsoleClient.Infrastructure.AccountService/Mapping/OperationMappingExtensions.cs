using ConsoleClient.Application.Model.Operations;
using ConsoleClient.Infrastructure.AccountService.Models.Operations;
using System.Diagnostics;

namespace ConsoleClient.Infrastructure.AccountService.Mapping;

public static class OperationMappingExtensions
{
    public static IOperation MapToModel(this BaseOperationEntry entry)
    {
        return entry switch
        {
            CheckBalanceOperationEntry checkBalance => checkBalance
                .MapToModel(),
            WithdrawOperationEntry withdraw => withdraw
                .MapToModel(),
            DepositOperationEntry deposit => deposit
                .MapToModel(),
            _ => throw new UnreachableException(
                $"mapping for type {entry.GetType().Name} is not implementing"),
        };
    }

    public static IOperation MapToModel(this CheckBalanceOperationEntry entry)
    {
        return new CheckBalanceOperation(
            entry.OccuredAt,
            entry.OperationId,
            entry.AccountId,
            entry.RemainingBalance);
    }

    public static IOperation MapToModel(this DepositOperationEntry entry)
    {
        return new DepositOperation(
            entry.OccuredAt,
            entry.OperationId,
            entry.AccountId,
            entry.Amount,
            entry.RemainingBalance);
    }

    public static IOperation MapToModel(this WithdrawOperationEntry entry)
    {
        return new WithdrawOperation(
            entry.OccuredAt,
            entry.OperationId,
            entry.AccountId,
            entry.Amount,
            entry.RemainingBalance);
    }
}