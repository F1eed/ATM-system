using Lab1.Application.Contracts.Operation.Operations;

namespace Lab1.Application.Contracts.Operation;

public interface IOperationService
{
    AddOperation.Response AddOperation(AddOperation.Request request);
}