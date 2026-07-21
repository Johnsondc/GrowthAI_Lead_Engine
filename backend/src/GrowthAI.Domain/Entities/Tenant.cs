// ============================================
// 鍔熻兘鎻忚堪锛氳璇佷笌澶氱鎴锋ā鍧?- 绉熸埛瀹炰綋
// 鐢熸垚锛歈oder by 搴勫洯
// 鐢熸垚鏃ユ湡锛?026-07-21
// ============================================

namespace GrowthAI.Domain.Entities;

/// <summary>
/// 绉熸埛/浼佷笟瀹炰綋
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