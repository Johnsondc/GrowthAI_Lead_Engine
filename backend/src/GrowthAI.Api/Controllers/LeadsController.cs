using GrowthAI.Application.Leads;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/leads")]
public sealed class LeadsController(ILeadService leadService) : ControllerBase
{
    private static readonly Guid DemoTenantId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    [HttpGet]
    public IActionResult List([FromQuery] string? status, [FromQuery] string? platform)
    {
        return Ok(leadService.List(DemoTenantId, status, platform));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateLeadCustomerRequest request)
    {
        var lead = leadService.Create(DemoTenantId, request);
        return Created($"/api/leads/{lead.Id}", lead);
    }

    [HttpPost("{id:guid}/status")]
    public IActionResult UpdateStatus(Guid id, [FromBody] UpdateLeadStatusRequest request)
    {
        var lead = leadService.UpdateStatus(DemoTenantId, id, request);
        return lead is null ? NotFound() : Ok(lead);
    }
}
