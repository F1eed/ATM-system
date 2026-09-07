namespace Service.Presentation.Http.Models;

public class CreateUserRequest
{
    public required string CardNumber { get; init; }

    public required string PinCode { get; init; }
}