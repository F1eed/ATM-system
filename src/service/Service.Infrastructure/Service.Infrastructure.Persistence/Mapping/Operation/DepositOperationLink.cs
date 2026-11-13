using Service.Application.Model.Accounts;
using Service.Application.Model.Operations;
using Service.Infrastructure.Persistence.Mapping.Operation.Results;
using Service.Infrastructure.Persistence.Models.Operation;

namespace Service.Infrastructure.Persistence.Mapping.Operation;

public class DepositOperationLink : OperationLinkBase
{
    public override MappingToDomainResult MapToDomain(
        DateTimeOffset occuredAt,
        long operationId,
        long accountId,
        OperationModel model)
    {
        return model switch
        {
            DepositOperationModel deposit => new MappingToDomainResult.Success(
                new DepositOperation(
                    new OccuredAt(occuredAt),
                    new OperationId(operationId),
                    new AccountId(accountId),
                    new Money(deposit.Amount))),
            _ => Next.MapToDomain(occuredAt, operationId, accountId, model),
        };
    }

    public override MappingToModelResult MapToModel(IOperation operation)
    {
        return operation switch
        {
            DepositOperation deposit => new MappingToModelResult.Success(
                new DepositOperationModel(deposit.Amount.Value)),
            _ => Next.MapToModel(operation),
        };
    }
}