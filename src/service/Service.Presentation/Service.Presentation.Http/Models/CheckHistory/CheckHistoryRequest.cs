using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Service.Presentation.Http.Models.CheckHistory;

public sealed class CheckHistoryRequest
{
    [FromQuery(Name = "pageToken")]
    public string? PageToken { get; init; }

    [FromQuery(Name = "pageSize")]
    [Range(1, int.MaxValue)]
    public int PageSize { get; init; }
}