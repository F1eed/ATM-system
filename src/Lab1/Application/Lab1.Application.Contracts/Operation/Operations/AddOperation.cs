using Lab1.Application.Contracts.Operation.Models;

namespace Lab1.Application.Contracts.Operation.Operations;

public static class AddOperation
{
    public readonly record struct Request(Guid SessionId, long AccountId);

    public readonly record struct Response(OperationDto Operation);
}