namespace Lab1.Application.Models.Operations;

public readonly record struct OperationId(long Value)
{
    public static readonly OperationId Default = new(default);
}