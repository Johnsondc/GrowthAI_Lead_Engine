// ============================================
// 功能描述：引流页模块 - DTO定义
// Sprint: 9 (M6 引流页)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Application.LandingPage.Dto;

/// <summary>
/// 引流页DTO
/// </summary>
public class LandingPageDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public string PageCode { get; set; } = string.Empty;
    public string? FormConfig { get; set; }
    public string? CustomCss { get; set; }
    public string? ThankYouMessage { get; set; }
    public string? RedirectUrl { get; set; }
    public int ViewCount { get; set; }
    public int SubmitCount { get; set; }
    public int Status { get; set; }
    public string AccessUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 创建引流页请求
/// </summary>
public class CreateLandingPageRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public string? FormConfig { get; set; }
    public string? CustomCss { get; set; }
    public string? ThankYouMessage { get; set; }
    public string? RedirectUrl { get; set; }
}

/// <summary>
/// 更新引流页请求
/// </summary>
public class UpdateLandingPageRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public string? FormConfig { get; set; }
    public string? CustomCss { get; set; }
    public string? ThankYouMessage { get; set; }
    public string? RedirectUrl { get; set; }
    public int? Status { get; set; }
}

/// <summary>
/// 引流页表单提交请求（公开接口）
/// </summary>
public class LandingPageSubmitRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? WeChat { get; set; }
    public string? City { get; set; }
    public string? ConsultContent { get; set; }
}

/// <summary>
/// 引流页统计DTO
/// </summary>
public class LandingPageStatsDto
{
    public long PageId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public int SubmitCount { get; set; }
    public double ConversionRate => ViewCount > 0 ? Math.Round((double)SubmitCount / ViewCount * 100, 2) : 0;
}
