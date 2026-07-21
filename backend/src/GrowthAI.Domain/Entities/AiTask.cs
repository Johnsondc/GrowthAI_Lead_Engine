// ============================================
// 功能描述：AI引擎模块 - AI任务实体
// Sprint: 4 (M5 AI引擎)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Domain.Enums;

namespace GrowthAI.Domain.Entities;

/// <summary>
/// AI任务执行记录
/// </summary>
public class AiTask
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public AiTaskType TaskType { get; set; }
    public string? InputParams { get; set; }
    public string? OutputResult { get; set; }
    public AiTaskStatus Status { get; set; } = AiTaskStatus.Queued;
    public long? PromptTemplateId { get; set; }
    public int CostTokens { get; set; }
    public int? DurationMs { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Tenant? Tenant { get; set; }
    public AiPromptTemplate? PromptTemplate { get; set; }
}
