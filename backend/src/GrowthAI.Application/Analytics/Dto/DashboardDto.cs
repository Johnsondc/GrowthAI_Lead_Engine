// ============================================
// 功能描述：数据看板模块 - DTO定义
// Sprint: 10 (M1 数据看板)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Application.Analytics.Dto;

/// <summary>
/// 看板概览数据
/// </summary>
public class DashboardOverviewDto
{
    public int TotalLeads { get; set; }
    public int NewLeadsToday { get; set; }
    public int HotLeads { get; set; }
    public int TotalContents { get; set; }
    public int LandingPageViews { get; set; }
    public int LandingPageSubmits { get; set; }
    public int ActiveUsers { get; set; }
}

/// <summary>
/// 客户趋势数据（按天/周/月）
/// </summary>
public class LeadTrendDto
{
    public string Date { get; set; } = string.Empty;
    public int Count { get; set; }
}

/// <summary>
/// 来源分布数据
/// </summary>
public class SourceDistributionDto
{
    public string SourceType { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}

/// <summary>
/// 销售排行
/// </summary>
public class SalesRankDto
{
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int LeadCount { get; set; }
    public int FollowUpCount { get; set; }
    public int HotCount { get; set; }
}

/// <summary>
/// 内容效果排行
/// </summary>
public class ContentPerformanceDto
{
    public long ContentId { get; set; }
    public string? Title { get; set; }
    public string? Platform { get; set; }
    public string? ContentType { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 看板查询请求
/// </summary>
public class DashboardQueryRequest
{
    public string Period { get; set; } = "Week"; // Day/Week/Month
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
