namespace GrowthAI.Application.Analytics;

public interface IAnalyticsService
{
    OverviewMetricsDto GetOverview(Guid tenantId);
}
