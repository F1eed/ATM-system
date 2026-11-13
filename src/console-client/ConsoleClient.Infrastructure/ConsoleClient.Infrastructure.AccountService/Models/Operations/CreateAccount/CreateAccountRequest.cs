namespace ConsoleClient.Infrastructure.AccountService.Models.Operations.CreateAccount;

public sealed record CreateAccountRequest(
    Guid SessionId,
    IReadOnlyCollection<CreateAccountRequest.AccountEntry> Entries)
{
    public sealed record AccountEntry(string CardNumber, string PinCode);
}