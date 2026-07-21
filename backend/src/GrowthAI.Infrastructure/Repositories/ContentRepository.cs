// ============================================
// 功能描述：内容中心模块 - 内容数据仓库（扩展查询）
// Sprint: 8 (M4 内容中心)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Content.Dto;
using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Infrastructure.Repositories;

public interface IContentRepository
{
    Task<ContentPagedResult> QueryAsync(long tenantId, ContentQueryRequest query);
    Task<AiContent?> GetByIdAsync(long id, long tenantId);
    Task<AiContent> CreateAsync(AiContent entity);
    Task<AiContent> UpdateAsync(AiContent entity);
    Task DeleteAsync(long id);
    Task<Dictionary<string, int>> GetStatisticsAsync(long tenantId);
}

public class ContentRepository : IContentRepository
{
    private readonly AppDbContext _db;

    public ContentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ContentPagedResult> QueryAsync(long tenantId, ContentQueryRequest query)
    {
        var q = _db.AiContents.Where(c => c.TenantId == tenantId && c.Status == 1);

        if (!string.IsNullOrEmpty(query.Platform))
            q = q.Where(c => c.TargetPlatform == query.Platform);
        if (!string.IsNullOrEmpty(query.ContentType))
            q = q.Where(c => c.ContentType == query.ContentType);
        if (!string.IsNullOrEmpty(query.Keyword))
            q = q.Where(c => (c.Title != null && c.Title.Contains(query.Keyword))
                          || (c.Body != null && c.Body.Contains(query.Keyword)));
        if (query.StartDate.HasValue)
            q = q.Where(c => c.CreatedAt >= query.StartDate.Value);
        if (query.EndDate.HasValue)
            q = q.Where(c => c.CreatedAt <= query.EndDate.Value);

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(c => c.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new ContentPagedResult
        {
            Items = items.Select(MapToDto).ToList(),
            Total = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<AiContent?> GetByIdAsync(long id, long tenantId)
    {
        return await _db.AiContents
            .Include(c => c.AiTask)
            .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId && c.Status == 1);
    }

    public async Task<AiContent> CreateAsync(AiContent entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        _db.AiContents.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<AiContent> UpdateAsync(AiContent entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _db.AiContents.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _db.AiContents.FindAsync(id);
        if (entity != null)
        {
            entity.Status = 0;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<Dictionary<string, int>> GetStatisticsAsync(long tenantId)
    {
        var contents = await _db.AiContents
            .Where(c => c.TenantId == tenantId && c.Status == 1)
            .GroupBy(c => c.TargetPlatform ?? "Unknown")
            .Select(g => new { Platform = g.Key, Count = g.Count() })
            .ToListAsync();

        var stats = contents.ToDictionary(x => x.Platform, x => x.Count);
        stats["Total"] = stats.Values.Sum();
        return stats;
    }

    private static ContentDto MapToDto(AiContent c)
    {
        return new ContentDto
        {
            Id = c.Id,
            ContentType = c.ContentType,
            TargetPlatform = c.TargetPlatform,
            Title = c.Title,
            Body = c.Body,
            Tags = c.Tags,
            Cta = c.Cta,
            Industry = c.Industry,
            Product = c.Product,
            City = c.City,
            AiTaskId = c.AiTaskId,
            CreatedAt = c.CreatedAt
        };
    }
}
