using Service.Application.Model.Accounts;
using Service.Application.Model.Operations;
using Service.Infrastructure.Persistence.Mapping.Operation.Results;
using Service.Infrastructure.Persistence.Models.Operation;

namespace Service.Infrastructure.Persistence.Mapping.Operation;

public class WithdrawOperationLink : OperationLinkBase
{
    public override MappingToDomainResult MapToDomain(
        DateTimeOffset occuredAt,
        long operationId,
        long accountId,
        OperationModel model)
    {
        if (model is WithdrawOperationModel withdraw)
        {
            return new MappingToDomainResult.Success(
                new WithdrawOperation(
                    new OccuredAt(occuredAt),
                    new OperationId(operationId),
                    new AccountId(accountId),
                    new Money(withdraw.Amount)));
        }

        return Next.MapToDomain(occuredAt, operationId, accountId, model);
    }

    public override MappingToModelResult MapToModel(IOperation operation)
    {
        if (operation is WithdrawOperation withdraw)
        {
            return new MappingToModelResult.Success(
                new WithdrawOperationModel(withdraw.Amount.Value));
        }

        return Next.MapToModel(operation);
    }
}