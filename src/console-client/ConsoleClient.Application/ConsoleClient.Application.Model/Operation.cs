namespace ConsoleClient.Application.Model;

public record Operation(long OperationId, long AccountId, string Type, decimal Balance);