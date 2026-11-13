namespace Service.Presentation.Http.Models.CreateAdmin;

public sealed class CreateAdminRequest
{
    public required string Password { get; init; }
}