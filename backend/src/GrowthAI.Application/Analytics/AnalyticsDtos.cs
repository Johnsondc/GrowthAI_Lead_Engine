namespace GrowthAI.Application.Analytics;

public sealed record OverviewMetricsDto(int TodayNewLeads, int ValidLeads, int HighIntentLeads, int WonLeads);
public sealed record SourceMetricDto(string Platform, int Count);
public sealed record FunnelMetricDto(string Status, int Count);
