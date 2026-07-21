// ============================================
// 功能描述：客户池模块 - 跟进记录数据仓库
// Sprint: 6 (M3 客户池)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Infrastructure.Repositories;

public interface IFollowUpRecordRepository
{
    Task<List<FollowUpRecord>> GetByLeadAsync(long leadCustomerId);
    Task<FollowUpRecord> CreateAsync(FollowUpRecord entity);
    Task<List<FollowUpRecord>> GetUpcomingFollowUpsAsync(long tenantId, DateTime before, int top = 20);
}

public class FollowUpRecordRepository : IFollowUpRecordRepository
{
    private readonly AppDbContext _db;

    public FollowUpRecordRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<FollowUpRecord>> GetByLeadAsync(long leadCustomerId)
    {
        return await _db.FollowUpRecords
            .Include(f => f.Follower)
            .Where(f => f.LeadCustomerId == leadCustomerId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<FollowUpRecord> CreateAsync(FollowUpRecord entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        _db.FollowUpRecords.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<List<FollowUpRecord>> GetUpcomingFollowUpsAsync(long tenantId, DateTime before, int top = 20)
    {
        return await _db.FollowUpRecords
            .Include(f => f.Follower)
            .Include(f => f.LeadCustomer)
            .Where(f => f.TenantId == tenantId
                     && f.NextFollowUpTime.HasValue
                     && f.NextFollowUpTime <= before)
            .OrderBy(f => f.NextFollowUpTime)
            .Take(top)
            .ToListAsync();
    }
}
