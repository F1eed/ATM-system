using Service.Application.Model.Accounts;

namespace Service.Application.Model.Operations;

public class DepositOperation : OperationBase
{
    public DepositOperation(OperationId operationId, AccountId accountId, decimal balance) : base(
        operationId,
        accountId,
        balance,
        OperationType.Deposit)
    { }
}