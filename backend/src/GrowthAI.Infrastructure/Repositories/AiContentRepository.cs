// ============================================
// 功能描述：AI引擎模块 - AI内容数据仓库
// Sprint: 4 (M5 AI引擎)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Infrastructure.Repositories;

public interface IAiContentRepository
{
    Task<AiContent?> GetByIdAsync(long id);
    Task<List<AiContent>> GetByTenantAsync(long tenantId, string? platform = null, int page = 1, int pageSize = 20);
    Task<int> CountByTenantAsync(long tenantId, string? platform = null);
    Task<AiContent> CreateAsync(AiContent content);
    Task<AiContent> UpdateAsync(AiContent content);
    Task DeleteAsync(long id);
}

public class AiContentRepository : IAiContentRepository
{
    private readonly AppDbContext _db;

    public AiContentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AiContent?> GetByIdAsync(long id)
    {
        return await _db.AiContents
            .Include(c => c.AiTask)
            .FirstOrDefaultAsync(c => c.Id == id && c.Status == 1);
    }

    public async Task<List<AiContent>> GetByTenantAsync(long tenantId, string? platform = null, int page = 1, int pageSize = 20)
    {
        var query = _db.AiContents.Where(c => c.TenantId == tenantId && c.Status == 1);
        if (!string.IsNullOrEmpty(platform))
            query = query.Where(c => c.TargetPlatform == platform);
        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountByTenantAsync(long tenantId, string? platform = null)
    {
        var query = _db.AiContents.Where(c => c.TenantId == tenantId && c.Status == 1);
        if (!string.IsNullOrEmpty(platform))
            query = query.Where(c => c.TargetPlatform == platform);
        return await query.CountAsync();
    }

    public async Task<AiContent> CreateAsync(AiContent content)
    {
        content.CreatedAt = DateTime.UtcNow;
        content.UpdatedAt = DateTime.UtcNow;
        _db.AiContents.Add(content);
        await _db.SaveChangesAsync();
        return content;
    }

    public async Task<AiContent> UpdateAsync(AiContent content)
    {
        content.UpdatedAt = DateTime.UtcNow;
        _db.AiContents.Update(content);
        await _db.SaveChangesAsync();
        return content;
    }

    public async Task DeleteAsync(long id)
    {
        var content = await _db.AiContents.FindAsync(id);
        if (content != null)
        {
            content.Status = 0;
            await _db.SaveChangesAsync();
        }
    }
}
