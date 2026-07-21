// ============================================
// 功能描述：内容中心模块 - 内容管理控制器
// Sprint: 8 (M4 内容中心)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Authorization;
using GrowthAI.Application.Content;
using GrowthAI.Application.Content.Dto;
using GrowthAI.Application.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/contents")]
[Authorize(Policy = RolePolicies.Authenticated)]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;

    public ContentController(IContentService contentService)
    {
        _contentService = contentService;
    }

    /// <summary>
    /// 分页查询内容列表（按平台/类型/关键词/日期筛选）
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Query([FromQuery] ContentQueryRequest query)
    {
        var tenantId = HttpContext.GetTenantId();
        var result = await _contentService.QueryAsync(tenantId, query);
        return Ok(result);
    }

    /// <summary>
    /// 获取内容详情
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var tenantId = HttpContext.GetTenantId();
        var dto = await _contentService.GetByIdAsync(tenantId, id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// 手动创建内容
    /// </summary>
    [HttpPost]
    [Authorize(Policy = RolePolicies.AdminOrOperator)]
    public async Task<IActionResult> Create([FromBody] CreateContentRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var dto = await _contentService.CreateAsync(tenantId, request);
        return Ok(dto);
    }

    /// <summary>
    /// 编辑内容（人工修改AI生成内容）
    /// </summary>
    [HttpPut("{id:long}")]
    [Authorize(Policy = RolePolicies.AdminOrOperator)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateContentRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var dto = await _contentService.UpdateAsync(tenantId, id, request);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// 删除内容（软删除）
    /// </summary>
    [HttpDelete("{id:long}")]
    [Authorize(Policy = RolePolicies.AdminOrOperator)]
    public async Task<IActionResult> Delete(long id)
    {
        var tenantId = HttpContext.GetTenantId();
        var success = await _contentService.DeleteAsync(tenantId, id);
        if (!success) return NotFound();
        return Ok(new { message = "已删除" });
    }

    /// <summary>
    /// 内容统计（按平台分组）
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var tenantId = HttpContext.GetTenantId();
        var stats = await _contentService.GetStatisticsAsync(tenantId);
        return Ok(stats);
    }
}
