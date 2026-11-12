using Service.Application.Model.Accounts;
using SourceKit.Generators.Builder.Annotations;

namespace Service.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public sealed partial record OperationQuery(
    AccountId AccountId,
    long? KeyCursor,
    [BuilderProperty(BuilderPropertyOptions.Required)]
    int PageSize);