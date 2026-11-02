using System.ComponentModel.DataAnnotations;

namespace Service.Application.Services.Options;

public class SessionServiceOptions
{
    [MinLength(1)]
    public required string Password { get; set; }
}
