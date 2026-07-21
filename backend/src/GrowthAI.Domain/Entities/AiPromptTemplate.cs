// ============================================
// 功能描述：AI引擎模块 - Prompt模板实体
// Sprint: 4 (M5 AI引擎)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Domain.Entities;

/// <summary>
/// AI Prompt模板 - 按行业+平台+内容类型管理提示词
/// </summary>
public class AiPromptTemplate
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string PromptText { get; set; } = string.Empty;
    public string? Variables { get; set; }
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
