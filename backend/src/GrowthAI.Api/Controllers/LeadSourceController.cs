// ============================================
// 功能描述：来源管理模块 - 来源控制器
// Sprint: 7 (M7 来源管理)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Authorization;
using GrowthAI.Application.LeadSource;
using GrowthAI.Application.LeadSource.Dto;
using GrowthAI.Application.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/sources")]
[Authorize(Policy = RolePolicies.Authenticated)]
public class LeadSourceController : ControllerBase
{
    private readonly ILeadSourceService _sourceService;

    public LeadSourceController(ILeadSourceService sourceService)
    {
        _sourceService = sourceService;
    }

    /// <summary>
    /// 获取来源列表
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tenantId = HttpContext.GetTenantId();
        var sources = await _sourceService.GetAllAsync(tenantId);
        return Ok(sources);
    }

    /// <summary>
    /// 获取单个来源
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var tenantId = HttpContext.GetTenantId();
        var dto = await _sourceService.GetByIdAsync(tenantId, id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// 新建来源（自动生成追踪码）
    /// </summary>
    [HttpPost]
    [Authorize(Policy = RolePolicies.AdminOrOwner)]
    public async Task<IActionResult> Create([FromBody] CreateLeadSourceRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var dto = await _sourceService.CreateAsync(tenantId, request);
        return Ok(dto);
    }

    /// <summary>
    /// 更新来源
    /// </summary>
    [HttpPut("{id:long}")]
    [Authorize(Policy = RolePolicies.AdminOrOwner)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateLeadSourceRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var dto = await _sourceService.UpdateAsync(tenantId, id, request);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// 停用来源（软删除）
    /// </summary>
    [HttpDelete("{id:long}")]
    [Authorize(Policy = RolePolicies.AdminOrOwner)]
    public async Task<IActionResult> Delete(long id)
    {
        var tenantId = HttpContext.GetTenantId();
        var success = await _sourceService.DeleteAsync(tenantId, id);
        if (!success) return NotFound();
        return Ok(new { message = "已停用" });
    }

    /// <summary>
    /// 通过追踪码查询来源（公开接口，用于引流页表单提交）
    /// </summary>
    [HttpGet("track/{code}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByTrackingCode(string code)
    {
        var dto = await _sourceService.GetByTrackingCodeAsync(code);
        if (dto == null) return NotFound();
        return Ok(dto);
    }
}
