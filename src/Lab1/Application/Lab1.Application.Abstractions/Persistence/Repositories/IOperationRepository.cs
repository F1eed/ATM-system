using Lab1.Application.Abstractions.Persistence.Queries;
using Lab1.Application.Models.Operations;

namespace Lab1.Application.Abstractions.Persistence.Repositories;

public interface IOperationRepository
{
    void Add(OperationBase operation);

    IEnumerable<OperationBase> Query(OperationQuery query);
}