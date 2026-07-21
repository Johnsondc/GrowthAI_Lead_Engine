// ============================================
// 功能描述：AI引擎模块 - AI服务接口
// Sprint: 4 (M5 AI引擎)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Domain.Entities;
using GrowthAI.Domain.Enums;

namespace GrowthAI.Application.Ai;

/// <summary>
/// AI服务接口 - 统一管理AI任务提交、执行、结果存储
/// </summary>
public interface IAiService
{
    /// <summary>
    /// 提交AI任务（选择模板 → 调用AI → 存储结果）
    /// </summary>
    Task<AiTaskResult> SubmitTaskAsync(long tenantId, AiTaskType taskType, AiTaskInput input);

    /// <summary>
    /// 查询AI任务结果
    /// </summary>
    Task<AiTaskResult?> GetTaskResultAsync(long taskId);

    /// <summary>
    /// 提交内容生成任务并直接返回生成的内容
    /// </summary>
    Task<AiGeneratedContent?> GenerateContentAsync(long tenantId, ContentGenerateRequest request);
}

/// <summary>
/// AI任务输入参数
/// </summary>
public class AiTaskInput
{
    public string Industry { get; set; } = "母婴";
    public string Platform { get; set; } = "小红书";
    public string ContentType { get; set; } = "ImageText";
    public string Product { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string TargetCustomer { get; set; } = string.Empty;
    public string SellingPoint { get; set; } = string.Empty;
    public string? ExtraParams { get; set; }
}

/// <summary>
/// AI任务执行结果
/// </summary>
public class AiTaskResult
{
    public long TaskId { get; set; }
    public AiTaskStatus Status { get; set; }
    public string? OutputContent { get; set; }
    public int TokensUsed { get; set; }
    public int DurationMs { get; set; }
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// 内容生成请求
/// </summary>
public class ContentGenerateRequest
{
    public string Industry { get; set; } = "母婴";
    public string Product { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string TargetCustomer { get; set; } = string.Empty;
    public string SellingPoint { get; set; } = string.Empty;
    public string Platform { get; set; } = "小红书";
    public string ContentType { get; set; } = "ImageText";
}

/// <summary>
/// AI生成的内容（解析后的结构化结果）
/// </summary>
public class AiGeneratedContent
{
    public long TaskId { get; set; }
    public long ContentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public string Cta { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}
