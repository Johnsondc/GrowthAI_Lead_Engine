// ============================================
// 功能描述：客户池模块 - 跟进记录实体
// Sprint: 6 (M3 客户池)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Domain.Entities;

/// <summary>
/// 销售跟进记录
/// </summary>
public class FollowUpRecord
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long LeadCustomerId { get; set; }
    public long FollowerId { get; set; }
    public string FollowType { get; set; } = "Phone";
    public string Content { get; set; } = string.Empty;
    public DateTime? NextFollowUpTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public LeadCustomer? LeadCustomer { get; set; }
    public AppUser? Follower { get; set; }
}
