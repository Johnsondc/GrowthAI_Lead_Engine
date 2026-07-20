using GrowthAI.Application.Analytics;
using GrowthAI.Domain.Enums;

namespace GrowthAI.Infrastructure.Persistence;

public sealed class AnalyticsService(InMemoryGrowthAiStore store) : IAnalyticsService
{
    public OverviewMetricsDto GetOverview(Guid tenantId)
    {
        var today = DateTimeOffset.UtcNow.Date;
        var leads = store.Leads.Where(lead => lead.TenantId == tenantId).ToList();
        return new OverviewMetricsDto(
            leads.Count(lead => lead.CreatedAt.Date == today),
            leads.Count(lead => lead.Status != LeadStatus.Invalid),
            leads.Count(lead => lead.Status == LeadStatus.HighIntent),
            leads.Count(lead => lead.Status == LeadStatus.Won));
    }
}
