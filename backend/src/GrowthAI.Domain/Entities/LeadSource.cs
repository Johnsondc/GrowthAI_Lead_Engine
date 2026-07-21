// ============================================
// 功能描述：来源管理模块 - 客户来源渠道实体
// Sprint: 7 (M7 来源管理)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Domain.Entities;

/// <summary>
/// 客户来源渠道
/// </summary>
public class LeadSource
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public string SourceType { get; set; } = "ManualInput";
    public string? Platform { get; set; }
    public string? AccountName { get; set; }
    public string? TrackingCode { get; set; }
    public long? LandingPageId { get; set; }
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Tenant? Tenant { get; set; }
}
