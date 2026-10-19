namespace Service.Presentation.Http.Models;

public sealed record CreateAccountResponse(IReadOnlyCollection<string> CardNumbers);
