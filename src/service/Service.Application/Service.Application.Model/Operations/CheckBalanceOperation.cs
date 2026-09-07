using Service.Application.Model.Accounts;

namespace Service.Application.Model.Operations;

public class CheckBalanceOperation : OperationBase
{
    public CheckBalanceOperation(OperationId operationId, AccountId accountId, decimal balance) : base(
        operationId,
        accountId,
        balance,
        OperationType.CheckBalance)
    { }
}