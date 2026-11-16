using Service.Application.Model.Accounts;
using Service.Application.Model.Operations;
using SourceKit.Generators.Builder.Annotations;

namespace Service.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public sealed partial record OperationQuery(
    AccountId AccountId,
    OperationId? KeyCursor,
    [BuilderProperty(BuilderPropertyOptions.Required)]
    int PageSize);