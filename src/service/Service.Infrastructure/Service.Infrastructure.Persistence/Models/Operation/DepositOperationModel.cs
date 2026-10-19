namespace Service.Infrastructure.Persistence.Models.Operation;

public sealed record DepositOperationModel(decimal Amount, decimal RemainingBalance) : OperationModel;
