using GrowthAI.Domain.Enums;

namespace GrowthAI.Application.Leads;

public sealed record LeadCustomerDto(
    Guid Id,
    string Name,
    string? Phone,
    string? Wechat,
    string? City,
    SourcePlatform SourcePlatform,
    string? SourceAccount,
    string? ConsultationContent,
    string? InterestedProduct,
    LeadStatus Status,
    Guid? OwnerUserId,
    DateTimeOffset CreatedAt);

public sealed record CreateLeadCustomerRequest(
    string Name,
    string? Phone,
    string? Wechat,
    string? City,
    SourcePlatform SourcePlatform,
    string? SourceAccount,
    string? ConsultationContent,
    string? InterestedProduct);

public sealed record UpdateLeadStatusRequest(LeadStatus Status, string? Remark);
