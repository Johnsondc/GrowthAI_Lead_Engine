// ============================================
// 功能描述：引流页模块 - 控制器
// Sprint: 9 (M6 引流页)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Authorization;
using GrowthAI.Application.LandingPage;
using GrowthAI.Application.LandingPage.Dto;
using GrowthAI.Application.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/landing-pages")]
[Authorize(Policy = RolePolicies.Authenticated)]
public class LandingPageController : ControllerBase
{
    private readonly ILandingPageService _pageService;

    public LandingPageController(ILandingPageService pageService)
    {
        _pageService = pageService;
    }

    /// <summary>
    /// 获取引流页列表
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tenantId = HttpContext.GetTenantId();
        var pages = await _pageService.GetAllAsync(tenantId);
        return Ok(pages);
    }

    /// <summary>
    /// 获取引流页详情
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var tenantId = HttpContext.GetTenantId();
        var dto = await _pageService.GetByIdAsync(tenantId, id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// 创建引流页（自动生成PageCode）
    /// </summary>
    [HttpPost]
    [Authorize(Policy = RolePolicies.AdminOrOperator)]
    public async Task<IActionResult> Create([FromBody] CreateLandingPageRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var dto = await _pageService.CreateAsync(tenantId, request);
        return Ok(dto);
    }

    /// <summary>
    /// 更新引流页
    /// </summary>
    [HttpPut("{id:long}")]
    [Authorize(Policy = RolePolicies.AdminOrOperator)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateLandingPageRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var dto = await _pageService.UpdateAsync(tenantId, id, request);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// 删除引流页（软删除）
    /// </summary>
    [HttpDelete("{id:long}")]
    [Authorize(Policy = RolePolicies.AdminOrOwner)]
    public async Task<IActionResult> Delete(long id)
    {
        var tenantId = HttpContext.GetTenantId();
        var success = await _pageService.DeleteAsync(tenantId, id);
        if (!success) return NotFound();
        return Ok(new { message = "已删除" });
    }

    /// <summary>
    /// 引流页统计（PV/提交/转化率）
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var tenantId = HttpContext.GetTenantId();
        var stats = await _pageService.GetStatisticsAsync(tenantId);
        return Ok(stats);
    }

    /// <summary>
    /// 公开接口：获取引流页内容（无需认证）
    /// </summary>
    [HttpGet("public/{pageCode}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicPage(string pageCode)
    {
        var dto = await _pageService.GetPublicPageAsync(pageCode);
        if (dto == null) return NotFound();

        // 记录PV
        await _pageService.RecordViewAsync(pageCode);
        return Ok(dto);
    }

    /// <summary>
    /// 公开接口：提交引流页表单（自动创建客户线索）
    /// </summary>
    [HttpPost("public/{pageCode}/submit")]
    [AllowAnonymous]
    public async Task<IActionResult> SubmitForm(string pageCode, [FromBody] LandingPageSubmitRequest request)
    {
        var dto = await _pageService.GetPublicPageAsync(pageCode);
        if (dto == null) return NotFound(new { message = "页面不存在或已下线" });

        var success = await _pageService.SubmitFormAsync(dto.Id, dto.Id, request);
        if (!success) return BadRequest(new { message = "提交失败" });

        return Ok(new { message = dto.ThankYouMessage ?? "提交成功", redirectUrl = dto.RedirectUrl });
    }
}
