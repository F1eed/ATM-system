namespace Service.Application.Contracts.Operation.Models;

public sealed record OperationDto(long OperationId, long AccountId, OperationTypeDto Type, decimal Balance);