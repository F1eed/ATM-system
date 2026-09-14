using Service.Application.Model.Accounts;

namespace Service.Application.Model.Operations;

public abstract class OperationBase
{
    // TODO: а нахуя OperationType, если у тебя и так есть наследники
    // можно оставить как вычисляемое свойство
    // тут тоже можно на уровне инфры сделать модельки, которые будут среиализоваываться в жсон потому что разные операции могут иметь разные атриубуты
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