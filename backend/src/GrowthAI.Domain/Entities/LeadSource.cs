using GrowthAI.Domain.Enums;

namespace GrowthAI.Domain.Entities;

public sealed class LeadSource : BaseEntity
{
    public SourcePlatform Platform { get; set; }
    public string? AccountName { get; set; }
    public string SourceCode { get; set; } = string.Empty;
    public string? LandingUrl { get; set; }
}
