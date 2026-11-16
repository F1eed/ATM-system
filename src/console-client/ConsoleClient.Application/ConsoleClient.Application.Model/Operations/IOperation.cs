namespace ConsoleClient.Application.Model.Operations;

public interface IOperation
{
    DateTimeOffset OccuredAt { get; }

    long OperationId { get; }

    long AccountId { get; }
}