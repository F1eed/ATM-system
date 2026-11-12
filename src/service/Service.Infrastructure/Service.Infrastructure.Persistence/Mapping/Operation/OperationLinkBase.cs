using Service.Application.Model.Operations;
using Service.Infrastructure.Persistence.Mapping.Operation.Results;
using Service.Infrastructure.Persistence.Models.Operation;

namespace Service.Infrastructure.Persistence.Mapping.Operation;

public abstract class OperationLinkBase : IOperationLink
{
    protected IOperationLink Next { get; private set; } = new DefaultOperationLink();

    public abstract MappingToDomainResult MapToDomain(
        DateTimeOffset occuredAt,
        long operationId,
        long accountId,
        OperationModel model);

    public abstract MappingToModelResult MapToModel(IOperation operation);

    public IOperationLink Add(IOperationLink link)
    {
        if (Next is DefaultOperationLink)
        {
            Next = link;

            return this;
        }

        Next.Add(link);

        return this;
    }
}