// ============================================
// 功能描述：客户池模块 - DTO定义
// Sprint: 6 (M3 客户池)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Application.Lead.Dto;

/// <summary>
/// 客户线索DTO（聚合模型）
/// </summary>
public class LeadCustomerDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? WeChat { get; set; }
    public string? City { get; set; }
    public string? SourcePlatform { get; set; }
    public string? SourceAccount { get; set; }
    public string? ConsultContent { get; set; }
    public string? InterestProduct { get; set; }
    public string Status { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty;
    public string? Remark { get; set; }
    public string? Tags { get; set; }
    public long? AssignedUserId { get; set; }
    public string? AssignedUserName { get; set; }
    public DateTime? LastFollowUpTime { get; set; }
    public string? InvalidReason { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 跟进记录DTO
/// </summary>
public class FollowUpRecordDto
{
    public long Id { get; set; }
    public string FollowType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string FollowerName { get; set; } = string.Empty;
    public DateTime? NextFollowUpTime { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 新建客户请求
/// </summary>
public class CreateLeadCustomerRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? WeChat { get; set; }
    public string? City { get; set; }
    public string? SourcePlatform { get; set; }
    public string? SourceAccount { get; set; }
    public string? ConsultContent { get; set; }
    public string? InterestProduct { get; set; }
    public string? Remark { get; set; }
    public string? Tags { get; set; }
    public long? AssignedUserId { get; set; }
}

/// <summary>
/// 更新客户请求
/// </summary>
public class UpdateLeadCustomerRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? WeChat { get; set; }
    public string? City { get; set; }
    public string? SourcePlatform { get; set; }
    public string? InterestProduct { get; set; }
    public string? Remark { get; set; }
    public string? Tags { get; set; }
}

/// <summary>
/// 状态更新请求
/// </summary>
public class UpdateLeadStatusRequest
{
    public string Status { get; set; } = string.Empty;
    public string? InvalidReason { get; set; }
}

/// <summary>
/// 分配负责人请求
/// </summary>
public class AssignLeadRequest
{
    public long AssignedUserId { get; set; }
}

/// <summary>
/// 新增跟进记录请求
/// </summary>
public class CreateFollowUpRequest
{
    public string FollowType { get; set; } = "Phone";
    public string Content { get; set; } = string.Empty;
    public DateTime? NextFollowUpTime { get; set; }
}

/// <summary>
/// 客户查询请求（分页+筛选）
/// </summary>
public class LeadQueryRequest
{
    public string? Status { get; set; }
    public string? SourcePlatform { get; set; }
    public long? AssignedUserId { get; set; }
    public string? City { get; set; }
    public string? Keyword { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// 分页结果
/// </summary>
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
