// ============================================
// 功能描述：AI引擎模块 - AI内容实体
// Sprint: 4 (M5 AI引擎)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Domain.Entities;

/// <summary>
/// AI生成的营销内容
/// </summary>
public class AiContent
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long? AiTaskId { get; set; }
    public string? ContentType { get; set; }
    public string? TargetPlatform { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public string? Tags { get; set; }
    public string? Cta { get; set; }
    public string? Industry { get; set; }
    public string? Product { get; set; }
    public string? City { get; set; }
    public long? LandingPageId { get; set; }
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Tenant? Tenant { get; set; }
    public AiTask? AiTask { get; set; }
}
