// ============================================
// 功能描述：引流页模块 - 数据仓库
// Sprint: 9 (M6 引流页)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Infrastructure.Repositories;

public interface ILandingPageRepository
{
    Task<List<LandingPage>> GetByTenantAsync(long tenantId);
    Task<LandingPage?> GetByIdAsync(long id, long tenantId);
    Task<LandingPage?> GetByPageCodeAsync(string pageCode);
    Task<LandingPage> CreateAsync(LandingPage entity);
    Task<LandingPage> UpdateAsync(LandingPage entity);
    Task DeleteAsync(long id);
    Task IncrementViewCountAsync(long id);
    Task IncrementSubmitCountAsync(long id);
}

public class LandingPageRepository : ILandingPageRepository
{
    private readonly AppDbContext _db;

    public LandingPageRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<LandingPage>> GetByTenantAsync(long tenantId)
    {
        return await _db.LandingPages
            .Where(p => p.TenantId == tenantId && p.Status == 1)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<LandingPage?> GetByIdAsync(long id, long tenantId)
    {
        return await _db.LandingPages
            .FirstOrDefaultAsync(p => p.Id == id && p.TenantId == tenantId);
    }

    public async Task<LandingPage?> GetByPageCodeAsync(string pageCode)
    {
        return await _db.LandingPages
            .FirstOrDefaultAsync(p => p.PageCode == pageCode && p.Status == 1);
    }

    public async Task<LandingPage> CreateAsync(LandingPage entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        _db.LandingPages.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<LandingPage> UpdateAsync(LandingPage entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _db.LandingPages.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _db.LandingPages.FindAsync(id);
        if (entity != null)
        {
            entity.Status = 0;
            await _db.SaveChangesAsync();
        }
    }

    public async Task IncrementViewCountAsync(long id)
    {
        await _db.LandingPages
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.ViewCount, p => p.ViewCount + 1));
    }

    public async Task IncrementSubmitCountAsync(long id)
    {
        await _db.LandingPages
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.SubmitCount, p => p.SubmitCount + 1));
    }
}
