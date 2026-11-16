namespace Service.Infrastructure.Persistence.Models.Operation;

public sealed record CheckBalanceOperationModel(decimal RemainingBalance) : OperationModel;