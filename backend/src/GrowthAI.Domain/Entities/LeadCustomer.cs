// ============================================
// 功能描述：客户池模块 - 客户线索实体
// Sprint: 6 (M3 客户池)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Domain.Entities;

/// <summary>
/// 客户线索 - 核心实体
/// </summary>
public class LeadCustomer
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? WeChat { get; set; }
    public string? City { get; set; }
    public string? SourcePlatform { get; set; }
    public string? SourceAccount { get; set; }
    public long? LeadSourceId { get; set; }
    public string? ConsultContent { get; set; }
    public string? InterestProduct { get; set; }
    public string Status { get; set; } = "New";
    public string SourceType { get; set; } = "ManualInput";
    public string? Remark { get; set; }
    public string? Tags { get; set; }
    public long? AssignedUserId { get; set; }
    public DateTime? LastFollowUpTime { get; set; }
    public string? InvalidReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Tenant? Tenant { get; set; }
    public AppUser? AssignedUser { get; set; }
}
