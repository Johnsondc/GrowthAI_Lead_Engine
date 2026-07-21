// ============================================
// 功能描述：引流页模块 - 引流页实体
// Sprint: 9 (M6 引流页)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Domain.Entities;

/// <summary>
/// 引流页/落地页
/// </summary>
public class LandingPage
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public string PageCode { get; set; } = string.Empty;
    public string? FormConfig { get; set; }
    public string? CustomCss { get; set; }
    public string? ThankYouMessage { get; set; }
    public string? RedirectUrl { get; set; }
    public int ViewCount { get; set; } = 0;
    public int SubmitCount { get; set; } = 0;
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Tenant? Tenant { get; set; }
}
