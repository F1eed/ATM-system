using Service.Application.Model.Accounts;
using Service.Application.Model.Operations;
using Service.Infrastructure.Persistence.Mapping.Operation.Results;
using Service.Infrastructure.Persistence.Models.Operation;

namespace Service.Infrastructure.Persistence.Mapping.Operation;

public class CheckBalanceOperationLink : OperationLinkBase
{
    public override MappingToDomainResult MapToDomain(
        DateTimeOffset occuredAt,
        long operationId,
        long accountId,
        OperationModel model)
    {
        return model switch
        {
            CheckBalanceOperationModel checkBalance => new MappingToDomainResult.Success(
                new CheckBalanceOperation(
                    new OccuredAt(occuredAt),
                    new OperationId(operationId),
                    new AccountId(accountId),
                    new Money(checkBalance.RemainingBalance))),
            _ => Next.MapToDomain(occuredAt, operationId, accountId, model),
        };
    }

    public override MappingToModelResult MapToModel(IOperation operation)
    {
        return operation switch
        {
            CheckBalanceOperation checkBalance => new MappingToModelResult.Success(
                new CheckBalanceOperationModel(checkBalance.Balance.Value)),
            _ => Next.MapToModel(operation),
        };
    }
}