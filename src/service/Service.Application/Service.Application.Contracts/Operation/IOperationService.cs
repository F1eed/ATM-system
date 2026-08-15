using Service.Application.Contracts.Operation.Operations;

namespace Service.Application.Contracts.Operation;

public interface IOperationService
{
    AddOperation.Response AddOperation(AddOperation.Request request);
}