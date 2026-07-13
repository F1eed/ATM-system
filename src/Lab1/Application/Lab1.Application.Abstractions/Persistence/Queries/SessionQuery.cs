using SourceKit.Generators.Builder.Annotations;

namespace Lab1.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public sealed partial record SessionQuery(Guid[] Ids);