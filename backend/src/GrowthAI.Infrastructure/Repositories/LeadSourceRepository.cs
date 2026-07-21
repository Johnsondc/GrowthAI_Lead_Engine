// ============================================
// 功能描述：来源管理模块 - 来源数据仓库
// Sprint: 7 (M7 来源管理)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Infrastructure.Repositories;

public interface ILeadSourceRepository
{
    Task<List<LeadSource>> GetByTenantAsync(long tenantId);
    Task<LeadSource?> GetByIdAsync(long id, long tenantId);
    Task<LeadSource?> GetByTrackingCodeAsync(string trackingCode);
    Task<LeadSource> CreateAsync(LeadSource entity);
    Task<LeadSource> UpdateAsync(LeadSource entity);
    Task DeleteAsync(long id);
}

public class LeadSourceRepository : ILeadSourceRepository
{
    private readonly AppDbContext _db;

    public LeadSourceRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<LeadSource>> GetByTenantAsync(long tenantId)
    {
        return await _db.LeadSources
            .Where(s => s.TenantId == tenantId && s.Status == 1)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<LeadSource?> GetByIdAsync(long id, long tenantId)
    {
        return await _db.LeadSources
            .FirstOrDefaultAsync(s => s.Id == id && s.TenantId == tenantId);
    }

    public async Task<LeadSource?> GetByTrackingCodeAsync(string trackingCode)
    {
        return await _db.LeadSources
            .FirstOrDefaultAsync(s => s.TrackingCode == trackingCode && s.Status == 1);
    }

    public async Task<LeadSource> CreateAsync(LeadSource entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        _db.LeadSources.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<LeadSource> UpdateAsync(LeadSource entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _db.LeadSources.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _db.LeadSources.FindAsync(id);
        if (entity != null)
        {
            entity.Status = 0;
            await _db.SaveChangesAsync();
        }
    }
}
