// ============================================
// 功能描述：来源管理模块 - DTO定义
// Sprint: 7 (M7 来源管理)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Application.LeadSource.Dto;

/// <summary>
/// 来源渠道DTO
/// </summary>
public class LeadSourceDto
{
    public long Id { get; set; }
    public string SourceType { get; set; } = string.Empty;
    public string? Platform { get; set; }
    public string? AccountName { get; set; }
    public string? TrackingCode { get; set; }
    public long? LandingPageId { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 新建来源请求
/// </summary>
public class CreateLeadSourceRequest
{
    public string SourceType { get; set; } = "ManualInput";
    public string? Platform { get; set; }
    public string? AccountName { get; set; }
    public long? LandingPageId { get; set; }
}

/// <summary>
/// 更新来源请求
/// </summary>
public class UpdateLeadSourceRequest
{
    public string? SourceType { get; set; }
    public string? Platform { get; set; }
    public string? AccountName { get; set; }
    public long? LandingPageId { get; set; }
}
