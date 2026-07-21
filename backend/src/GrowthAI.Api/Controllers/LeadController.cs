// ============================================
// 功能描述：客户池模块 - 客户管理控制器
// Sprint: 6 (M3 客户池)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using System.Security.Claims;
using GrowthAI.Application.Authorization;
using GrowthAI.Application.Lead;
using GrowthAI.Application.Lead.Dto;
using GrowthAI.Application.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/leads")]
[Authorize(Policy = RolePolicies.Authenticated)]
public class LeadController : ControllerBase
{
    private readonly ILeadService _leadService;

    public LeadController(ILeadService leadService)
    {
        _leadService = leadService;
    }

    /// <summary>
    /// 分页查询客户列表
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Query([FromQuery] LeadQueryRequest query)
    {
        var tenantId = HttpContext.GetTenantId();
        var result = await _leadService.QueryAsync(tenantId, query);
        return Ok(result);
    }

    /// <summary>
    /// 获取客户详情
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var tenantId = HttpContext.GetTenantId();
        var dto = await _leadService.GetByIdAsync(tenantId, id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// 新建客户
    /// </summary>
    [HttpPost]
    [Authorize(Policy = RolePolicies.AdminOrSales)]
    public async Task<IActionResult> Create([FromBody] CreateLeadCustomerRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var userId = GetCurrentUserId();
        try
        {
            var dto = await _leadService.CreateAsync(tenantId, userId, request);
            return Ok(dto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// 更新客户信息
    /// </summary>
    [HttpPut("{id:long}")]
    [Authorize(Policy = RolePolicies.AdminOrSales)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateLeadCustomerRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var dto = await _leadService.UpdateAsync(tenantId, id, request);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// 更新客户状态（状态流转校验）
    /// </summary>
    [HttpPut("{id:long}/status")]
    [Authorize(Policy = RolePolicies.AdminOrSales)]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateLeadStatusRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        try
        {
            var dto = await _leadService.UpdateStatusAsync(tenantId, id, request);
            if (dto == null) return NotFound();
            return Ok(dto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// 分配负责人
    /// </summary>
    [HttpPut("{id:long}/assign")]
    [Authorize(Policy = RolePolicies.AdminOrOwner)]
    public async Task<IActionResult> Assign(long id, [FromBody] AssignLeadRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        try
        {
            var dto = await _leadService.AssignAsync(tenantId, id, request);
            if (dto == null) return NotFound();
            return Ok(dto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// 获取跟进记录
    /// </summary>
    [HttpGet("{id:long}/followups")]
    public async Task<IActionResult> GetFollowUps(long id)
    {
        var tenantId = HttpContext.GetTenantId();
        var records = await _leadService.GetFollowUpsAsync(tenantId, id);
        return Ok(records);
    }

    /// <summary>
    /// 新增跟进记录
    /// </summary>
    [HttpPost("{id:long}/followups")]
    [Authorize(Policy = RolePolicies.AdminOrSales)]
    public async Task<IActionResult> CreateFollowUp(long id, [FromBody] CreateFollowUpRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var userId = GetCurrentUserId();
        try
        {
            var dto = await _leadService.CreateFollowUpAsync(tenantId, userId, id, request);
            return Ok(dto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// 获取热门客户
    /// </summary>
    [HttpGet("hot")]
    public async Task<IActionResult> GetHotLeads([FromQuery] int top = 10)
    {
        var tenantId = HttpContext.GetTenantId();
        var leads = await _leadService.GetHotLeadsAsync(tenantId, top);
        return Ok(leads);
    }

    /// <summary>
    /// 客户统计（各状态数量）
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var tenantId = HttpContext.GetTenantId();
        var stats = await _leadService.GetStatisticsAsync(tenantId);
        return Ok(stats);
    }

    private long GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null ? long.Parse(claim.Value) : 0;
    }
}
