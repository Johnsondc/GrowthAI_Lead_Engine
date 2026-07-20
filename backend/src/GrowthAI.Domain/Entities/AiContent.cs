namespace GrowthAI.Domain.Entities;

public sealed class AiContent : BaseEntity
{
    public Guid? AiTaskId { get; set; }
    public string Platform { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? ProductName { get; set; }
    public string? City { get; set; }
    public string? TargetAudience { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public string? Script { get; set; }
    public string? Tags { get; set; }
    public string? CallToAction { get; set; }
}
