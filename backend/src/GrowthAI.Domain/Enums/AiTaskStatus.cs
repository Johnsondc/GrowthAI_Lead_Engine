// ============================================
// 功能描述：AI引擎模块 - AI任务状态枚举
// Sprint: 4 (M5 AI引擎)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Domain.Enums;

/// <summary>
/// AI任务执行状态
/// </summary>
public enum AiTaskStatus
{
    /// <summary>排队中</summary>
    Queued = 0,
    /// <summary>执行中</summary>
    Running = 1,
    /// <summary>成功</summary>
    Success = 2,
    /// <summary>失败</summary>
    Failed = 3
}
