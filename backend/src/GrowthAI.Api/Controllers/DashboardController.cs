// ============================================
// 功能描述：数据看板模块 - 控制器
// Sprint: 10 (M1 数据看板)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Analytics;
using GrowthAI.Application.Analytics.Dto;
using GrowthAI.Application.Authorization;
using GrowthAI.Application.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Policy = RolePolicies.Authenticated)]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// 获取看板概览数据
    /// </summary>
    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        var tenantId = HttpContext.GetTenantId();
        var data = await _dashboardService.GetOverviewAsync(tenantId);
        return Ok(data);
    }

    /// <summary>
    /// 客户新增趋势（按天）
    /// </summary>
    [HttpGet("lead-trend")]
    public async Task<IActionResult> GetLeadTrend([FromQuery] DashboardQueryRequest query)
    {
        var tenantId = HttpContext.GetTenantId();
        var data = await _dashboardService.GetLeadTrendAsync(tenantId, query);
        return Ok(data);
    }

    /// <summary>
    /// 来源分布
    /// </summary>
    [HttpGet("source-distribution")]
    public async Task<IActionResult> GetSourceDistribution()
    {
        var tenantId = HttpContext.GetTenantId();
        var data = await _dashboardService.GetSourceDistributionAsync(tenantId);
        return Ok(data);
    }

    /// <summary>
    /// 销售排行榜
    /// </summary>
    [HttpGet("sales-rank")]
    [Authorize(Policy = RolePolicies.AdminOrOwner)]
    public async Task<IActionResult> GetSalesRank([FromQuery] int top = 5)
    {
        var tenantId = HttpContext.GetTenantId();
        var data = await _dashboardService.GetSalesRankAsync(tenantId, top);
        return Ok(data);
    }

    /// <summary>
    /// 最近内容表现
    /// </summary>
    [HttpGet("recent-contents")]
    public async Task<IActionResult> GetRecentContents([FromQuery] int top = 10)
    {
        var tenantId = HttpContext.GetTenantId();
        var data = await _dashboardService.GetRecentContentsAsync(tenantId, top);
        return Ok(data);
    }
}
