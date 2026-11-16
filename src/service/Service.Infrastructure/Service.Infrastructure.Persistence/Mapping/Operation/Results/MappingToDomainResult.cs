using Service.Application.Model.Operations;

namespace Service.Infrastructure.Persistence.Mapping.Operation.Results;

public abstract record MappingToDomainResult
{
    private MappingToDomainResult()
    { }

    public sealed record Success(IOperation Operation) : MappingToDomainResult;

    public sealed record Failure(string ErrorMessage) : MappingToDomainResult;
}