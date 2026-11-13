namespace Service.Infrastructure.Persistence.Models.Operation;

public sealed record WithdrawOperationModel(decimal Amount) : OperationModel;