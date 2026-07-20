using GrowthAI.Domain.Enums;

namespace GrowthAI.Domain.Entities;

public sealed class LeadCustomer : BaseEntity
{
    public Guid? SourceId { get; set; }
    public Guid? OwnerUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Wechat { get; set; }
    public string? City { get; set; }
    public SourcePlatform SourcePlatform { get; set; }
    public string? SourceAccount { get; set; }
    public string? ConsultationContent { get; set; }
    public string? InterestedProduct { get; set; }
    public LeadStatus Status { get; set; } = LeadStatus.New;
    public string? Remark { get; set; }

    public bool HasContactMethod => !string.IsNullOrWhiteSpace(Phone) || !string.IsNullOrWhiteSpace(Wechat);
}
