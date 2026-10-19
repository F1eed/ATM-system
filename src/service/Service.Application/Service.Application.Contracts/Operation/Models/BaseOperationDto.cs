using System.Text.Json.Serialization;

namespace Service.Application.Contracts.Operation.Models;

[JsonDerivedType(typeof(DepositOperationDto))]
[JsonDerivedType(typeof(WithdrawOperationDto))]
[JsonDerivedType(typeof(CheckBalanceOperationDto))]
public abstract record BaseOperationDto(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt,
    string Type);