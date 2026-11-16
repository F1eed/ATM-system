using Service.Application.Model.Operations;
using Service.Infrastructure.Persistence.Mapping.Operation.Results;
using Service.Infrastructure.Persistence.Models.Operation;

namespace Service.Infrastructure.Persistence.Mapping.Operation;

public class DefaultOperationLink : IOperationLink
{
    public MappingToDomainResult MapToDomain(
        DateTimeOffset occuredAt,
        long operationId,
        long accountId,
        OperationModel model)
    {
        return new MappingToDomainResult.Failure("Default link don't map to domain");
    }

    public MappingToModelResult MapToModel(IOperation operation)
    {
        return new MappingToModelResult.Failure("Default link don't map to model");
    }

    public IOperationLink Add(IOperationLink link)
    {
        throw new InvalidOperationException("Can't add the next link");
    }
}