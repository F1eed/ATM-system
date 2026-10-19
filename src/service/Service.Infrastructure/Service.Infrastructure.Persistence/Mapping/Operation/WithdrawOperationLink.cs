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
        return model switch
        {
            WithdrawOperationModel withdraw => new MappingToDomainResult.Success(
                new WithdrawOperation(
                    new OccuredAt(occuredAt),
                    new OperationId(operationId),
                    new AccountId(accountId),
                    new Money(withdraw.Amount),
                    new Money(withdraw.RemainingBalance))),
            _ => Next.MapToDomain(occuredAt, operationId, accountId, model),
        };
    }

    public override MappingToModelResult MapToModel(IOperation operation)
    {
        return operation switch
        {
            WithdrawOperation withdraw => new MappingToModelResult.Success(
                new WithdrawOperationModel(
                    withdraw.Amount.Value,
                    withdraw.RemainingBalance.Value)),
            _ => Next.MapToModel(operation),
        };
    }
}