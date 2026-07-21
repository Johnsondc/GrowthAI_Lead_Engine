// ============================================
// 功能描述：AI引擎模块 - AI任务数据仓库
// Sprint: 4 (M5 AI引擎)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Domain.Entities;
using GrowthAI.Domain.Enums;
using GrowthAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Infrastructure.Repositories;

public interface IAiTaskRepository
{
    Task<AiTask?> GetByIdAsync(long id);
    Task<List<AiTask>> GetByTenantAsync(long tenantId, AiTaskStatus? status = null, int page = 1, int pageSize = 20);
    Task<int> CountByTenantAsync(long tenantId, AiTaskStatus? status = null);
    Task<AiTask> CreateAsync(AiTask task);
    Task<AiTask> UpdateAsync(AiTask task);
}

public class AiTaskRepository : IAiTaskRepository
{
    private readonly AppDbContext _db;

    public AiTaskRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AiTask?> GetByIdAsync(long id)
    {
        return await _db.AiTasks
            .Include(t => t.PromptTemplate)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<AiTask>> GetByTenantAsync(long tenantId, AiTaskStatus? status = null, int page = 1, int pageSize = 20)
    {
        var query = _db.AiTasks.Where(t => t.TenantId == tenantId);
        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);
        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountByTenantAsync(long tenantId, AiTaskStatus? status = null)
    {
        var query = _db.AiTasks.Where(t => t.TenantId == tenantId);
        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);
        return await query.CountAsync();
    }

    public async Task<AiTask> CreateAsync(AiTask task)
    {
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;
        _db.AiTasks.Add(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task<AiTask> UpdateAsync(AiTask task)
    {
        task.UpdatedAt = DateTime.UtcNow;
        _db.AiTasks.Update(task);
        await _db.SaveChangesAsync();
        return task;
    }
}
