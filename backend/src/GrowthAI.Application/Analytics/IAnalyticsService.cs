namespace GrowthAI.Application.Analytics;

public interface IAnalyticsService
{
    OverviewMetricsDto GetOverview(Guid tenantId);
    IReadOnlyList<SourceMetricDto> GetSources(Guid tenantId);
    IReadOnlyList<FunnelMetricDto> GetFunnel(Guid tenantId);
}
