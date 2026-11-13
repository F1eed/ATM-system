using ConsoleClient.Infrastructure.AccountService.Models.Operations.CheckBalance;
using ConsoleClient.Infrastructure.AccountService.Models.Operations.Deposit;
using ConsoleClient.Infrastructure.AccountService.Models.Operations.Withdraw;
using System.Text.Json.Serialization;

namespace ConsoleClient.Infrastructure.AccountService.Models.Operations;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(CheckBalanceOperationEntry), "check balance")]
[JsonDerivedType(typeof(WithdrawOperationEntry), "withdraw")]
[JsonDerivedType(typeof(DepositOperationEntry), "deposit")]
public abstract record BaseOperationEntry(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt);