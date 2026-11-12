using System.Text.Json.Serialization;

namespace ConsoleClient.Application.Contracts.Models.Operations;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(CheckBalanceOperationDto), "check balance")]
[JsonDerivedType(typeof(WithdrawOperationDto), "withdraw")]
[JsonDerivedType(typeof(DepositOperationDto), "deposit")]
public abstract record BaseOperationDto(DateTimeOffset OccuredAt);