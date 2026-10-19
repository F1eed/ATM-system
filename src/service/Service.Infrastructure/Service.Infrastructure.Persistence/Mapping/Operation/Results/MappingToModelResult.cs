using Service.Infrastructure.Persistence.Models.Operation;

namespace Service.Infrastructure.Persistence.Mapping.Operation.Results;

public abstract record MappingToModelResult
{
    private MappingToModelResult()
    { }

    public sealed record Success(OperationModel Model) : MappingToModelResult;

    public sealed record Failure(string ErrorMessage) : MappingToModelResult;
}
