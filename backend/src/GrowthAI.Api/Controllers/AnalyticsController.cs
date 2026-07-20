using GrowthAI.Application.Analytics;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/analytics")]
public sealed class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
{
    private static readonly Guid DemoTenantId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    [HttpGet("overview")]
    public IActionResult Overview()
    {
        return Ok(analyticsService.GetOverview(DemoTenantId));
    }
}
