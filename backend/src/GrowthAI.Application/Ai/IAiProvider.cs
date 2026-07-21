// ============================================
// 功能描述：AI引擎模块 - AI Provider 统一接口
// Sprint: 4 (M5 AI引擎)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Application.Ai;

/// <summary>
/// AI Provider 统一调用接口（后续可替换为 OpenAI / 通义千问 / 文心等）
/// </summary>
public interface IAiProvider
{
    /// <summary>
    /// 发送消息给AI，返回生成的文本结果
    /// </summary>
    /// <param name="systemPrompt">系统提示词（Prompt模板内容）</param>
    /// <param name="userMessage">用户输入内容</param>
    /// <returns>AI生成的文本结果</returns>
    Task<AiProviderResult> SendMessageAsync(string systemPrompt, string userMessage);
}

/// <summary>
/// AI Provider 返回结果
/// </summary>
public class AiProviderResult
{
    public bool Success { get; set; }
    public string Content { get; set; } = string.Empty;
    public int TokensUsed { get; set; }
    public string? ErrorMessage { get; set; }
    public int DurationMs { get; set; }
}
