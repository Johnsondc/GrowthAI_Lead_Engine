namespace GrowthAI.Domain.Entities;

public sealed class AiTask : BaseEntity
{
    public string Provider { get; set; } = "DeepSeek";
    public string TaskType { get; set; } = "ContentGeneration";
    public string InputJson { get; set; } = "{}";
    public string Status { get; set; } = "Pending";
    public string? ErrorMessage { get; set; }
}
