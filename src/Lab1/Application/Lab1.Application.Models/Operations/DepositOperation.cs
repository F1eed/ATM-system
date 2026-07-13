using Lab1.Application.Models.Accounts;

namespace Lab1.Application.Models.Operations;

public class DepositOperation : OperationBase
{
    public DepositOperation(OperationId operationId, AccountId accountId, decimal balance) : base(
        operationId,
        accountId,
        balance,
        OperationType.Deposit)
    {
    }
}