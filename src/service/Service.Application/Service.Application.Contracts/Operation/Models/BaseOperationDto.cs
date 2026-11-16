using System.Text.Json.Serialization;

namespace Service.Application.Contracts.Operation.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(DepositOperationDto), "deposit")]
[JsonDerivedType(typeof(WithdrawOperationDto), "withdraw")]
[JsonDerivedType(typeof(CheckBalanceOperationDto), "check balance")]
public abstract record BaseOperationDto(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt);