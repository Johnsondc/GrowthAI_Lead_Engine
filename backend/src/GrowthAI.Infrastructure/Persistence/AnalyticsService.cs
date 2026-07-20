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

    public IReadOnlyList<SourceMetricDto> GetSources(Guid tenantId)
    {
        return store.Leads
            .Where(lead => lead.TenantId == tenantId)
            .GroupBy(lead => lead.SourcePlatform.ToString())
            .Select(group => new SourceMetricDto(group.Key, group.Count()))
            .OrderByDescending(item => item.Count)
            .ToList();
    }

    public IReadOnlyList<FunnelMetricDto> GetFunnel(Guid tenantId)
    {
        return Enum.GetValues<LeadStatus>()
            .Select(status => new FunnelMetricDto(status.ToString(), store.Leads.Count(lead => lead.TenantId == tenantId && lead.Status == status)))
            .ToList();
    }
}
