using System.Text.Json.Serialization;

namespace Service.Infrastructure.Persistence.Models.Operation;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(DepositOperationModel), "deposit")]
[JsonDerivedType(typeof(CheckBalanceOperationModel), "check_balance")]
[JsonDerivedType(typeof(WithdrawOperationModel), "withdraw")]
public abstract record OperationModel;
