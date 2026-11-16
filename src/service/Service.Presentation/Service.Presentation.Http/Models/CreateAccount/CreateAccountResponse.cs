namespace Service.Presentation.Http.Models.CreateAccount;

public sealed record CreateAccountResponse(IReadOnlyCollection<string> CardNumbers);