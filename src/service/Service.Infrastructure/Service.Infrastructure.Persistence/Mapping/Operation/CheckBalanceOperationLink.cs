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
        if (model is CheckBalanceOperationModel checkBalance)
        {
            return new MappingToDomainResult.Success(
                new CheckBalanceOperation(
                    new OccuredAt(occuredAt),
                    new OperationId(operationId),
                    new AccountId(accountId),
                    new Money(checkBalance.RemainingBalance)));
        }

        return Next.MapToDomain(occuredAt, operationId, accountId, model);
    }

    public override MappingToModelResult MapToModel(IOperation operation)
    {
        if (operation is CheckBalanceOperation checkBalance)
        {
            return new MappingToModelResult.Success(
                new CheckBalanceOperationModel(checkBalance.RemainingBalance.Value));
        }

        return Next.MapToModel(operation);
    }
}