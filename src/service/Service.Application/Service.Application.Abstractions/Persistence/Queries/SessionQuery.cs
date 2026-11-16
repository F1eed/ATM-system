using SourceKit.Generators.Builder.Annotations;

namespace Service.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public sealed partial record SessionQuery(Guid[] Ids);