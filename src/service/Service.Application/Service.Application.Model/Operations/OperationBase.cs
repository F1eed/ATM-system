using Service.Application.Model.Accounts;

namespace Service.Application.Model.Operations;

public abstract class OperationBase
{
    protected OperationBase(OperationId operationId, AccountId accountId, decimal balance, OperationType type)
    {
        OperationId = operationId;
        AccountId = accountId;
        Balance = balance;
        Type = type;
    }

    public AccountId AccountId { get; }

    public OperationId OperationId { get; }

    public decimal Balance { get; }

    public OperationType Type { get; }
}