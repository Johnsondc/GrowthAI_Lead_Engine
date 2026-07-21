// ============================================
// 功能描述：企业管理模块 - DTO定义
// Sprint: 5 (M2 企业管理)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Application.Enterprise.Dto;

/// <summary>
/// 企业信息DTO
/// </summary>
public class EnterpriseInfoDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? ContactPhone { get; set; }
    public string PlanType { get; set; } = "Basic";
    public int Status { get; set; }
}

/// <summary>
/// 更新企业信息请求
/// </summary>
public class UpdateEnterpriseRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? ContactPhone { get; set; }
}
