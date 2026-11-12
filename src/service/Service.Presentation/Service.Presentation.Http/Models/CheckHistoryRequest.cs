using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Service.Presentation.Http.Models;

public sealed class CheckHistoryRequest
{
    [FromQuery(Name = "pageToken")]
    public string? PageToken { get; init; }

    [FromQuery(Name = "pageSize")]
    [Range(1, 10)]
    public int PageSize { get; init; }
}