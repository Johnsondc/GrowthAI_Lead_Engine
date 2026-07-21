// ============================================
// 功能描述：数据看板模块 - 看板服务
// Sprint: 10 (M1 数据看板)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Analytics.Dto;
using GrowthAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Application.Analytics;

public interface IDashboardService
{
    Task<DashboardOverviewDto> GetOverviewAsync(long tenantId);
    Task<List<LeadTrendDto>> GetLeadTrendAsync(long tenantId, DashboardQueryRequest query);
    Task<List<SourceDistributionDto>> GetSourceDistributionAsync(long tenantId);
    Task<List<SalesRankDto>> GetSalesRankAsync(long tenantId, int top = 5);
    Task<List<ContentPerformanceDto>> GetRecentContentsAsync(long tenantId, int top = 10);
}

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _db;

    public DashboardService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardOverviewDto> GetOverviewAsync(long tenantId)
    {
        var today = DateTime.UtcNow.Date;
        var totalLeads = await _db.LeadCustomers.CountAsync(l => l.TenantId == tenantId);
        var newToday = await _db.LeadCustomers.CountAsync(l => l.TenantId == tenantId && l.CreatedAt >= today);
        var hotLeads = await _db.LeadCustomers.CountAsync(l => l.TenantId == tenantId && l.Status == "Hot");
        var totalContents = await _db.AiContents.CountAsync(c => c.TenantId == tenantId && c.Status == 1);
        var pageViews = await _db.LandingPages.Where(p => p.TenantId == tenantId).SumAsync(p => p.ViewCount);
        var pageSubmits = await _db.LandingPages.Where(p => p.TenantId == tenantId).SumAsync(p => p.SubmitCount);
        var activeUsers = await _db.AppUsers.CountAsync(u => u.TenantId == tenantId && u.Status == 1);

        return new DashboardOverviewDto
        {
            TotalLeads = totalLeads,
            NewLeadsToday = newToday,
            HotLeads = hotLeads,
            TotalContents = totalContents,
            LandingPageViews = pageViews,
            LandingPageSubmits = pageSubmits,
            ActiveUsers = activeUsers
        };
    }

    public async Task<List<LeadTrendDto>> GetLeadTrendAsync(long tenantId, DashboardQueryRequest query)
    {
        var startDate = query.StartDate ?? GetStartDate(query.Period);
        var endDate = query.EndDate ?? DateTime.UtcNow;

        var leads = await _db.LeadCustomers
            .Where(l => l.TenantId == tenantId && l.CreatedAt >= startDate && l.CreatedAt <= endDate)
            .GroupBy(l => l.CreatedAt.Date)
            .Select(g => new LeadTrendDto
            {
                Date = g.Key.ToString("yyyy-MM-dd"),
                Count = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return leads;
    }

    public async Task<List<SourceDistributionDto>> GetSourceDistributionAsync(long tenantId)
    {
        var sources = await _db.LeadCustomers
            .Where(l => l.TenantId == tenantId)
            .GroupBy(l => l.SourceType)
            .Select(g => new SourceDistributionDto
            {
                SourceType = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        var total = sources.Sum(s => s.Count);
        foreach (var s in sources)
        {
            s.Percentage = total > 0 ? Math.Round((double)s.Count / total * 100, 2) : 0;
        }

        return sources.OrderByDescending(s => s.Count).ToList();
    }

    public async Task<List<SalesRankDto>> GetSalesRankAsync(long tenantId, int top = 5)
    {
        var users = await _db.AppUsers
            .Where(u => u.TenantId == tenantId && u.Status == 1)
            .ToListAsync();

        var ranks = new List<SalesRankDto>();
        foreach (var user in users)
        {
            var leadCount = await _db.LeadCustomers.CountAsync(l => l.AssignedUserId == user.Id);
            var followUpCount = await _db.FollowUpRecords.CountAsync(f => f.FollowerId == user.Id);
            var hotCount = await _db.LeadCustomers.CountAsync(l => l.AssignedUserId == user.Id && l.Status == "Hot");

            ranks.Add(new SalesRankDto
            {
                UserId = user.Id,
                UserName = user.Name,
                LeadCount = leadCount,
                FollowUpCount = followUpCount,
                HotCount = hotCount
            });
        }

        return ranks.OrderByDescending(r => r.LeadCount).Take(top).ToList();
    }

    public async Task<List<ContentPerformanceDto>> GetRecentContentsAsync(long tenantId, int top = 10)
    {
        return await _db.AiContents
            .Where(c => c.TenantId == tenantId && c.Status == 1)
            .OrderByDescending(c => c.CreatedAt)
            .Take(top)
            .Select(c => new ContentPerformanceDto
            {
                ContentId = c.Id,
                Title = c.Title,
                Platform = c.TargetPlatform,
                ContentType = c.ContentType,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();
    }

    private static DateTime GetStartDate(string period)
    {
        return period switch
        {
            "Day" => DateTime.UtcNow.AddDays(-1),
            "Week" => DateTime.UtcNow.AddDays(-7),
            "Month" => DateTime.UtcNow.AddMonths(-1),
            _ => DateTime.UtcNow.AddDays(-7)
        };
    }
}
