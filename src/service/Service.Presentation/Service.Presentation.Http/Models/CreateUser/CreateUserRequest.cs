using System.ComponentModel.DataAnnotations;

namespace Service.Presentation.Http.Models.CreateUser;

public class CreateUserRequest
{
    [Required]
    [Length(minimumLength: 16, maximumLength: 16)]
    public string CardNumber { get; init; } = string.Empty;

    [Required]
    [Length(minimumLength: 4, maximumLength: 4)]
    public string PinCode { get; init; } = string.Empty;
}