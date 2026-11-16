using System.ComponentModel.DataAnnotations;

namespace Service.Application.Services.Options;

public class SessionServiceOptions
{
    [MinLength(1)]
    [Required]
    public string Password { get; set; } = string.Empty;
}