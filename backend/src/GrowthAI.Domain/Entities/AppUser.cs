// ============================================
// 功能描述：认证与多租户模块 - 系统用户实体
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Domain.Enums;

namespace GrowthAI.Domain.Entities;

/// <summary>
/// 系统用户实体
/// </summary>
public class AppUser
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Operator;
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Tenant? Tenant { get; set; }
}
