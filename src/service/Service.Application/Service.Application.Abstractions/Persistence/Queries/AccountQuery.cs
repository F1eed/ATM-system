using Service.Application.Model.Accounts;
using SourceKit.Generators.Builder.Annotations;

namespace Service.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public sealed partial record AccountQuery(
    IReadOnlyList<AccountId> Ids,
    IReadOnlyList<string> CardNumbers);