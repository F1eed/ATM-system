namespace Service.Presentation.Http.Models;

public sealed record CreateUserResponse(
    IReadOnlyCollection<CreateUserResponse.UserSessionEntry> Entries)
{
    public sealed record UserSessionEntry(Guid Id);
}
