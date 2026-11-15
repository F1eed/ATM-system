using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Service.Presentation.Http.Models.CheckHistory;

public sealed class CheckHistoryRequest
{
    [FromQuery(Name = "pageToken")]
    public string? PageToken { get; init; }

    [FromQuery(Name = "pageSize")]
    [Range(minimum: 1, maximum: 1000)]
    public int PageSize { get; init; }
}