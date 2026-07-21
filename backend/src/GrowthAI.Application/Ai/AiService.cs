// ============================================
// 功能描述：AI引擎模块 - AI服务实现
// Sprint: 4 (M5 AI引擎)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using System.Text.Json;
using GrowthAI.Domain.Entities;
using GrowthAI.Domain.Enums;
using GrowthAI.Infrastructure.Repositories;

namespace GrowthAI.Application.Ai;

/// <summary>
/// AI服务实现 - 统一管理AI任务流程
/// </summary>
public class AiService : IAiService
{
    private readonly IAiProvider _aiProvider;
    private readonly IAiPromptTemplateRepository _templateRepo;
    private readonly IAiTaskRepository _taskRepo;
    private readonly IAiContentRepository _contentRepo;

    // 简易频次控制（V1阶段，后续替换为Redis）
    private static readonly Dictionary<long, int> _dailyCallCount = new();
    private static DateTime _lastResetDate = DateTime.UtcNow.Date;
    private const int MaxDailyCallsPerTenant = 100;

    public AiService(
        IAiProvider aiProvider,
        IAiPromptTemplateRepository templateRepo,
        IAiTaskRepository taskRepo,
        IAiContentRepository contentRepo)
    {
        _aiProvider = aiProvider;
        _templateRepo = templateRepo;
        _taskRepo = taskRepo;
        _contentRepo = contentRepo;
    }

    public async Task<AiTaskResult> SubmitTaskAsync(long tenantId, AiTaskType taskType, AiTaskInput input)
    {
        // 1. 频次控制
        if (!CheckRateLimit(tenantId))
        {
            return new AiTaskResult
            {
                Status = AiTaskStatus.Failed,
                ErrorMessage = "今日AI调用次数已达上限，请明天再试"
            };
        }

        // 2. 选择Prompt模板
        var template = await _templateRepo.FindMatchAsync(input.Industry, input.Platform, input.ContentType);
        var systemPrompt = template?.PromptText ?? GetDefaultPrompt(input.ContentType);

        // 3. 构造用户消息
        var userMessage = BuildUserMessage(input);

        // 4. 创建任务记录
        var task = new AiTask
        {
            TenantId = tenantId,
            TaskType = taskType,
            InputParams = JsonSerializer.Serialize(input),
            PromptTemplateId = template?.Id,
            Status = AiTaskStatus.Running
        };
        task = await _taskRepo.CreateAsync(task);

        // 5. 调用AI
        try
        {
            var result = await _aiProvider.SendMessageAsync(systemPrompt, userMessage);

            task.Status = result.Success ? AiTaskStatus.Success : AiTaskStatus.Failed;
            task.OutputResult = result.Content;
            task.CostTokens = result.TokensUsed;
            task.DurationMs = result.DurationMs;
            task.ErrorMessage = result.ErrorMessage;
            await _taskRepo.UpdateAsync(task);

            IncrementCallCount(tenantId);

            return new AiTaskResult
            {
                TaskId = task.Id,
                Status = task.Status,
                OutputContent = result.Content,
                TokensUsed = result.TokensUsed,
                DurationMs = result.DurationMs,
                ErrorMessage = result.ErrorMessage
            };
        }
        catch (Exception ex)
        {
            task.Status = AiTaskStatus.Failed;
            task.ErrorMessage = ex.Message;
            await _taskRepo.UpdateAsync(task);

            return new AiTaskResult
            {
                TaskId = task.Id,
                Status = AiTaskStatus.Failed,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<AiTaskResult?> GetTaskResultAsync(long taskId)
    {
        var task = await _taskRepo.GetByIdAsync(taskId);
        if (task == null) return null;

        return new AiTaskResult
        {
            TaskId = task.Id,
            Status = task.Status,
            OutputContent = task.OutputResult,
            TokensUsed = task.CostTokens,
            DurationMs = task.DurationMs ?? 0,
            ErrorMessage = task.ErrorMessage
        };
    }

    public async Task<AiGeneratedContent?> GenerateContentAsync(long tenantId, ContentGenerateRequest request)
    {
        var input = new AiTaskInput
        {
            Industry = request.Industry,
            Platform = request.Platform,
            ContentType = request.ContentType,
            Product = request.Product,
            City = request.City,
            TargetCustomer = request.TargetCustomer,
            SellingPoint = request.SellingPoint
        };

        var taskResult = await SubmitTaskAsync(tenantId, AiTaskType.ContentGeneration, input);

        if (taskResult.Status != AiTaskStatus.Success || string.IsNullOrEmpty(taskResult.OutputContent))
            return null;

        // 解析AI返回的JSON内容
        var parsed = ParseAiOutput(taskResult.OutputContent);

        // 存储到AiContent表
        var content = new AiContent
        {
            TenantId = tenantId,
            AiTaskId = taskResult.TaskId,
            ContentType = request.ContentType,
            TargetPlatform = request.Platform,
            Title = parsed.Title,
            Body = parsed.Body,
            Tags = JsonSerializer.Serialize(parsed.Tags),
            Cta = parsed.Cta,
            Industry = request.Industry,
            Product = request.Product,
            City = request.City
        };
        content = await _contentRepo.CreateAsync(content);

        return new AiGeneratedContent
        {
            TaskId = taskResult.TaskId,
            ContentId = content.Id,
            Title = parsed.Title,
            Body = parsed.Body,
            Tags = parsed.Tags,
            Cta = parsed.Cta,
            Platform = request.Platform,
            ContentType = request.ContentType
        };
    }

    // === 私有方法 ===

    private string BuildUserMessage(AiTaskInput input)
    {
        return $"行业：{input.Industry}\n产品：{input.Product}\n城市：{input.City}\n目标客户：{input.TargetCustomer}\n核心卖点：{input.SellingPoint}";
    }

    private string GetDefaultPrompt(string contentType)
    {
        return contentType switch
        {
            "ShortVideoScript" => "你是一位专业的短视频脚本撰写师，擅长为母婴品牌创作种草类短视频脚本。请生成包含分镜建议的短视频脚本，以JSON格式返回。",
            "MomentPost" => "你是一位朋友圈文案专家，擅长为母婴品牌撰写口语化、有互动引导的朋友圈文案。请以JSON格式返回。",
            _ => "你是一位专业的新媒体内容创作师，擅长为母婴品牌创作种草类图文内容。请生成标题、正文、标签和CTA，以JSON格式返回，包含title、body、tags(数组)、cta四个字段。"
        };
    }

    private (string Title, string Body, List<string> Tags, string Cta) ParseAiOutput(string output)
    {
        try
        {
            var doc = JsonDocument.Parse(output);
            var root = doc.RootElement;

            var title = root.TryGetProperty("title", out var t) ? t.GetString() ?? "" : "";
            var body = root.TryGetProperty("body", out var b) ? b.GetString() ?? "" : "";
            var cta = root.TryGetProperty("cta", out var c) ? c.GetString() ?? "" : "";

            var tags = new List<string>();
            if (root.TryGetProperty("tags", out var tagsElement) && tagsElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var tag in tagsElement.EnumerateArray())
                {
                    var tagStr = tag.GetString();
                    if (!string.IsNullOrEmpty(tagStr)) tags.Add(tagStr);
                }
            }

            return (title, body, tags, cta);
        }
        catch
        {
            return ("AI生成内容", output, new List<string>(), "");
        }
    }

    // === 简易频次控制 ===

    private bool CheckRateLimit(long tenantId)
    {
        ResetIfNeeded();
        _dailyCallCount.TryGetValue(tenantId, out var count);
        return count < MaxDailyCallsPerTenant;
    }

    private void IncrementCallCount(long tenantId)
    {
        ResetIfNeeded();
        if (!_dailyCallCount.ContainsKey(tenantId))
            _dailyCallCount[tenantId] = 0;
        _dailyCallCount[tenantId]++;
    }

    private void ResetIfNeeded()
    {
        if (DateTime.UtcNow.Date > _lastResetDate)
        {
            _dailyCallCount.Clear();
            _lastResetDate = DateTime.UtcNow.Date;
        }
    }
}
