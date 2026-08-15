namespace Service.Presentation.Http.Models;

public sealed class CreateAdminRequest
{
    public required string Password { get; init; }
}