// ============================================
// 功能描述：认证与多租户模块 - 租户数据仓库
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Infrastructure.Repositories;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(long id);
    Task<Tenant?> GetByNameAsync(string name);
    Task<Tenant> CreateAsync(Tenant tenant);
    Task<Tenant> UpdateAsync(Tenant tenant);
}

public class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _db;

    public TenantRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Tenant?> GetByIdAsync(long id)
    {
        return await _db.Tenants.FindAsync(id);
    }

    public async Task<Tenant?> GetByNameAsync(string name)
    {
        return await _db.Tenants.FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<Tenant> CreateAsync(Tenant tenant)
    {
        tenant.CreatedAt = DateTime.UtcNow;
        tenant.UpdatedAt = DateTime.UtcNow;
        _db.Tenants.Add(tenant);
        await _db.SaveChangesAsync();
        return tenant;
    }

    public async Task<Tenant> UpdateAsync(Tenant tenant)
    {
        tenant.UpdatedAt = DateTime.UtcNow;
        _db.Tenants.Update(tenant);
        await _db.SaveChangesAsync();
        return tenant;
    }
}
