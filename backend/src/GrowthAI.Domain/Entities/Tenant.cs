// ============================================
// 功能描述：认证与多租户模块 - 租户实体
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Domain.Entities;

/// <summary>
/// 租户/企业实体
/// </summary>
public class Tenant
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? ContactPhone { get; set; }
    public string PlanType { get; set; } = "Basic";
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
