using Lab1.Application.Models.Accounts;
using SourceKit.Generators.Builder.Annotations;

namespace Lab1.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public sealed partial record OperationQuery(AccountId Id);