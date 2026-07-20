using GrowthAI.Application.Leads;
using GrowthAI.Domain.Entities;
using GrowthAI.Domain.Enums;

namespace GrowthAI.Infrastructure.Persistence;

public sealed class LeadService(InMemoryGrowthAiStore store) : ILeadService
{
    public IReadOnlyList<LeadCustomerDto> List(Guid tenantId, string? status, string? platform)
    {
        var query = store.Leads.Where(lead => lead.TenantId == tenantId);

        if (Enum.TryParse<LeadStatus>(status, true, out var parsedStatus))
        {
            query = query.Where(lead => lead.Status == parsedStatus);
        }

        if (Enum.TryParse<SourcePlatform>(platform, true, out var parsedPlatform))
        {
            query = query.Where(lead => lead.SourcePlatform == parsedPlatform);
        }

        return query.OrderByDescending(lead => lead.CreatedAt).Select(ToDto).ToList();
    }

    public LeadCustomerDto Create(Guid tenantId, CreateLeadCustomerRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Phone) && string.IsNullOrWhiteSpace(request.Wechat))
        {
            throw new ArgumentException("手机号或微信至少填写一个");
        }

        var lead = new LeadCustomer
        {
            TenantId = tenantId,
            Name = request.Name,
            Phone = request.Phone,
            Wechat = request.Wechat,
            City = request.City,
            SourcePlatform = request.SourcePlatform,
            SourceAccount = request.SourceAccount,
            ConsultationContent = request.ConsultationContent,
            InterestedProduct = request.InterestedProduct,
            Status = LeadStatus.New
        };

        store.Leads.Add(lead);
        return ToDto(lead);
    }

    public LeadCustomerDto? UpdateStatus(Guid tenantId, Guid leadId, UpdateLeadStatusRequest request)
    {
        var lead = store.Leads.FirstOrDefault(item => item.TenantId == tenantId && item.Id == leadId);
        if (lead is null)
        {
            return null;
        }

        lead.Status = request.Status;
        lead.Remark = request.Remark ?? lead.Remark;
        lead.UpdatedAt = DateTimeOffset.UtcNow;
        return ToDto(lead);
    }

    private static LeadCustomerDto ToDto(LeadCustomer lead) => new(
        lead.Id,
        lead.Name,
        lead.Phone,
        lead.Wechat,
        lead.City,
        lead.SourcePlatform,
        lead.SourceAccount,
        lead.ConsultationContent,
        lead.InterestedProduct,
        lead.Status,
        lead.OwnerUserId,
        lead.CreatedAt);
}
