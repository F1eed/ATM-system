using Lab1.Application.Models.Accounts;

namespace Lab1.Application.Models.Operations;

public class CheckBalanceOperation : OperationBase
{
    public CheckBalanceOperation(OperationId operationId, AccountId accountId, decimal balance) : base(
        operationId,
        accountId,
        balance,
        OperationType.CheckBalance)
    {
    }
}