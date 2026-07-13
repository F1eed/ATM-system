using Lab1.Application.Abstractions.Persistence.Queries;
using Lab1.Application.Abstractions.Persistence.Repositories;
using Lab1.Application.Models.Operations;

namespace Lab1.Infrastructure.Persistence.Repositories;

internal sealed class OperationRepository : IOperationRepository
{
    private readonly List<OperationBase> _values = [];

    public void Add(OperationBase operation)
    {
        operation = operation.Type switch
        {
            OperationType.Deposit => new DepositOperation(
                new OperationId(_values.Count + 1),
                operation.AccountId,
                operation.Balance),
            OperationType.Withdraw => new WithdrawOperation(
                new OperationId(_values.Count + 1),
                operation.AccountId,
                operation.Balance),
            OperationType.CheckBalance => new CheckBalanceOperation(
                new OperationId(_values.Count + 1),
                operation.AccountId,
                operation.Balance),
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        };
        _values.Add(operation);
    }

    public IEnumerable<OperationBase> Query(OperationQuery query)
    {
        return _values.FindAll(x => x.AccountId == query.Id);
    }
}