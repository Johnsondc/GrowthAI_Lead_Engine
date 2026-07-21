// ============================================
// 功能描述：AI引擎模块 - Prompt模板数据仓库
// Sprint: 4 (M5 AI引擎)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Infrastructure.Repositories;

public interface IAiPromptTemplateRepository
{
    Task<AiPromptTemplate?> GetByIdAsync(long id);
    Task<AiPromptTemplate?> FindMatchAsync(string industry, string platform, string contentType);
    Task<List<AiPromptTemplate>> GetAllAsync();
    Task<AiPromptTemplate> CreateAsync(AiPromptTemplate template);
    Task<AiPromptTemplate> UpdateAsync(AiPromptTemplate template);
}

public class AiPromptTemplateRepository : IAiPromptTemplateRepository
{
    private readonly AppDbContext _db;

    public AiPromptTemplateRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AiPromptTemplate?> GetByIdAsync(long id)
    {
        return await _db.AiPromptTemplates.FindAsync(id);
    }

    public async Task<AiPromptTemplate?> FindMatchAsync(string industry, string platform, string contentType)
    {
        return await _db.AiPromptTemplates
            .Where(t => t.Status == 1)
            .OrderByDescending(t =>
                (t.Industry == industry ? 2 : 0) +
                (t.Platform == platform ? 2 : 0) +
                (t.ContentType == contentType ? 2 : 0))
            .FirstOrDefaultAsync();
    }

    public async Task<List<AiPromptTemplate>> GetAllAsync()
    {
        return await _db.AiPromptTemplates
            .Where(t => t.Status == 1)
            .OrderBy(t => t.Industry).ThenBy(t => t.Platform)
            .ToListAsync();
    }

    public async Task<AiPromptTemplate> CreateAsync(AiPromptTemplate template)
    {
        template.CreatedAt = DateTime.UtcNow;
        template.UpdatedAt = DateTime.UtcNow;
        _db.AiPromptTemplates.Add(template);
        await _db.SaveChangesAsync();
        return template;
    }

    public async Task<AiPromptTemplate> UpdateAsync(AiPromptTemplate template)
    {
        template.UpdatedAt = DateTime.UtcNow;
        _db.AiPromptTemplates.Update(template);
        await _db.SaveChangesAsync();
        return template;
    }
}
