// ============================================
// 功能描述：企业管理模块 - 企业设置控制器
// Sprint: 5 (M2 企业管理)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Authorization;
using GrowthAI.Application.Enterprise;
using GrowthAI.Application.Enterprise.Dto;
using GrowthAI.Application.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/settings")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly IEnterpriseService _enterpriseService;

    public SettingsController(IEnterpriseService enterpriseService)
    {
        _enterpriseService = enterpriseService;
    }

    /// <summary>
    /// 获取企业信息
    /// </summary>
    [HttpGet("enterprise")]
    public async Task<IActionResult> GetEnterprise()
    {
        var tenantId = HttpContext.GetTenantId();
        var result = await _enterpriseService.GetEnterpriseAsync(tenantId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// 更新企业信息（仅管理员）
    /// </summary>
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [HttpPut("enterprise")]
    public async Task<IActionResult> UpdateEnterprise([FromBody] UpdateEnterpriseRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var result = await _enterpriseService.UpdateEnterpriseAsync(tenantId, request);
        if (result == null) return NotFound();
        return Ok(result);
    }
}
