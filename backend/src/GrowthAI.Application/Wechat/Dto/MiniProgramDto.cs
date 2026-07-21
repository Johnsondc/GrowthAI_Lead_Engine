// ============================================
// 功能描述：微信小程序模块 - DTO定义
// Sprint: 11 (M8 微信小程序)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Application.Wechat.Dto;

/// <summary>
/// 小程序登录请求
/// </summary>
public class MiniProgramLoginRequest
{
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// 小程序登录响应
/// </summary>
public class MiniProgramLoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public long TenantId { get; set; }
}

/// <summary>
/// 小程序客户提交表单（公开）
/// </summary>
public class MiniLeadSubmitRequest
{
    public string PageCode { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? WeChat { get; set; }
    public string? City { get; set; }
    public string? ConsultContent { get; set; }
}

/// <summary>
/// 小程序客户提交响应
/// </summary>
public class MiniLeadSubmitResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
