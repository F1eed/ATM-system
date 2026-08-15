namespace Service.Infrastructure.Persistence.Models.Operation;

public record OperationDao(long OperationId, long AccountId, string Type, decimal Balance);