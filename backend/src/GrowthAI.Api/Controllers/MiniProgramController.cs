// ============================================
// 功能描述：微信小程序模块 - 公开API控制器
// Sprint: 11 (M8 微信小程序)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Auth;
using GrowthAI.Application.LandingPage;
using GrowthAI.Application.LandingPage.Dto;
using GrowthAI.Application.Wechat.Dto;
using GrowthAI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/mini")]
public class MiniProgramController : ControllerBase
{
    private readonly ILandingPageService _landingPageService;
    private readonly ILeadSourceRepository _sourceRepo;
    private readonly JwtHelper _jwtHelper;

    public MiniProgramController(
        ILandingPageService landingPageService,
        ILeadSourceRepository sourceRepo,
        JwtHelper jwtHelper)
    {
        _landingPageService = landingPageService;
        _sourceRepo = sourceRepo;
        _jwtHelper = jwtHelper;
    }

    /// <summary>
    /// 小程序登录（wx.login code换取session）
    /// 注：实际生产环境需调用微信code2session接口验证
    /// 此处为Mock实现，直接返回token
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] MiniProgramLoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Code))
            return BadRequest(new { message = "code不能为空" });

        // Mock: 实际应调用微信 code2session 接口
        // var session = await _wechatService.GetSessionAsync(request.Code);
        // 这里简化处理，返回一个临时token用于后续接口调用

        return Ok(new
        {
            message = "登录成功(Mock)",
            sessionKey = Guid.NewGuid().ToString("N"),
            openid = $"mock_openid_{request.Code}"
        });
    }

    /// <summary>
    /// 获取引流页内容（小程序访问）
    /// </summary>
    [HttpGet("page/{pageCode}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPage(string pageCode)
    {
        var page = await _landingPageService.GetPublicPageAsync(pageCode);
        if (page == null) return NotFound(new { message = "页面不存在或已下线" });

        await _landingPageService.RecordViewAsync(pageCode);
        return Ok(page);
    }

    /// <summary>
    /// 提交客户信息（小程序表单提交，自动创建线索）
    /// </summary>
    [HttpPost("submit")]
    [AllowAnonymous]
    public async Task<IActionResult> SubmitLead([FromBody] MiniLeadSubmitRequest request)
    {
        if (string.IsNullOrEmpty(request.PageCode))
            return BadRequest(new { success = false, message = "pageCode不能为空" });

        var page = await _landingPageService.GetPublicPageAsync(request.PageCode);
        if (page == null)
            return NotFound(new { success = false, message = "页面不存在或已下线" });

        var submitRequest = new LandingPageSubmitRequest
        {
            Name = request.Name,
            Phone = request.Phone,
            WeChat = request.WeChat,
            City = request.City,
            ConsultContent = request.ConsultContent
        };

        var success = await _landingPageService.SubmitFormAsync(page.Id, page.Id, submitRequest);

        return Ok(new MiniLeadSubmitResponse
        {
            Success = success,
            Message = success ? "提交成功，我们将尽快联系您" : "提交失败，请稍后重试"
        });
    }

    /// <summary>
    /// 获取来源信息（通过追踪码）
    /// </summary>
    [HttpGet("source/{trackingCode}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSource(string trackingCode)
    {
        var source = await _sourceRepo.GetByTrackingCodeAsync(trackingCode);
        if (source == null) return NotFound(new { message = "来源不存在" });

        return Ok(new
        {
            source.Id,
            source.SourceType,
            source.Platform,
            source.AccountName
        });
    }
}
