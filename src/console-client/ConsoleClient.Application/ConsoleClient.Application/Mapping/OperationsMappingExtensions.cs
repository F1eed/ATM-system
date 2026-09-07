using ConsoleClient.Application.Contracts.Models;
using ConsoleClient.Application.Model;

namespace ConsoleClient.Application.Mapping;

public static class OperationsMappingExtensions
{
    public static OperationDto MapToDto(this Operation operation)
        => new OperationDto(operation.Type, operation.Balance);
}