using Service.Application.Model.Operations;
using Service.Infrastructure.Persistence.Mapping.Operation.Results;
using Service.Infrastructure.Persistence.Models.Operation;

namespace Service.Infrastructure.Persistence.Mapping.Operation;

public interface IOperationLink
{
    MappingToDomainResult MapToDomain(
        DateTimeOffset occuredAt,
        long operationId,
        long accountId,
        OperationModel model);

    MappingToModelResult MapToModel(IOperation operation);

    IOperationLink Add(IOperationLink link);
}