namespace ConsoleClient.Infrastructure.AccountService.Models.Operations.CreateAccount;

// А что вот тут возвращать если не модельку?
public record CreateAccountResponse(IReadOnlyCollection<string> CardNumbers);