using Service.Application.Model.Accounts;

namespace Service.Application.Model.Operations;

public interface IOperation
{
    OperationId OperationId { get; }

    AccountId AccountId { get; }

    OccuredAt OccuredAt { get; }
}