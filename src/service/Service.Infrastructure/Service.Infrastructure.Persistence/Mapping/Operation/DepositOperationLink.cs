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
        if (model is DepositOperationModel deposit)
        {
            return new MappingToDomainResult.Success(
                new DepositOperation(
                    new OccuredAt(occuredAt),
                    new OperationId(operationId),
                    new AccountId(accountId),
                    new Money(deposit.Amount)));
        }

        return Next.MapToDomain(occuredAt, operationId, accountId, model);
    }

    public override MappingToModelResult MapToModel(IOperation operation)
    {
        if (operation is DepositOperation deposit)
        {
            return new MappingToModelResult.Success(
                new DepositOperationModel(deposit.Amount.Value));
        }

        return Next.MapToModel(operation);
    }
}