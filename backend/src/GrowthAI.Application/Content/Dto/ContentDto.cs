// ============================================
// 功能描述：内容中心模块 - DTO定义
// Sprint: 8 (M4 内容中心)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Application.Content.Dto;

/// <summary>
/// 内容DTO
/// </summary>
public class ContentDto
{
    public long Id { get; set; }
    public string? ContentType { get; set; }
    public string? TargetPlatform { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public string? Tags { get; set; }
    public string? Cta { get; set; }
    public string? Industry { get; set; }
    public string? Product { get; set; }
    public string? City { get; set; }
    public long? AiTaskId { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 内容查询请求
/// </summary>
public class ContentQueryRequest
{
    public string? Platform { get; set; }
    public string? ContentType { get; set; }
    public string? Keyword { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// 更新内容请求（人工编辑AI生成内容）
/// </summary>
public class UpdateContentRequest
{
    public string? Title { get; set; }
    public string? Body { get; set; }
    public string? Tags { get; set; }
    public string? Cta { get; set; }
    public string? TargetPlatform { get; set; }
}

/// <summary>
/// 手动创建内容请求
/// </summary>
public class CreateContentRequest
{
    public string ContentType { get; set; } = "ImageText";
    public string TargetPlatform { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    public string? Tags { get; set; }
    public string? Cta { get; set; }
    public string? Product { get; set; }
    public string? City { get; set; }
}

/// <summary>
/// 内容分页结果
/// </summary>
public class ContentPagedResult
{
    public List<ContentDto> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
