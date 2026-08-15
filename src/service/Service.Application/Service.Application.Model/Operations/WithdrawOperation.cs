using Service.Application.Model.Accounts;

namespace Service.Application.Model.Operations;

public class WithdrawOperation : OperationBase
{
    public WithdrawOperation(OperationId operationId, AccountId accountId, decimal balance) : base(
        operationId,
        accountId,
        balance,
        OperationType.Withdraw) { }
}