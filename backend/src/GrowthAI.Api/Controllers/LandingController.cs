using GrowthAI.Application.Leads;
using GrowthAI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/landing")]
public sealed class LandingController(ILeadService leadService) : ControllerBase
{
    private static readonly Guid DemoTenantId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    [HttpGet("{code}")]
    public IActionResult Get(string code)
    {
        return Ok(new
        {
            code,
            title = "成都月嫂怎么选？3 分钟获取适合你的月嫂方案",
            fields = new[] { "name", "phone", "wechat", "city", "interestedProduct", "consultationContent" }
        });
    }

    [HttpPost("{code}/submit")]
    public IActionResult Submit(string code, [FromBody] CreateLeadCustomerRequest request)
    {
        var normalized = request with
        {
            SourcePlatform = request.SourcePlatform == default ? SourcePlatform.H5Form : request.SourcePlatform,
            SourceAccount = string.IsNullOrWhiteSpace(request.SourceAccount) ? code : request.SourceAccount
        };
        var lead = leadService.Create(DemoTenantId, normalized);
        return Created($"/api/leads/{lead.Id}", lead);
    }
}
