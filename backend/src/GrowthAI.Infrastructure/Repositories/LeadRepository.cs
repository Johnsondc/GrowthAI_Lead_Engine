// ============================================
// 功能描述：客户池模块 - 客户线索数据仓库
// Sprint: 6 (M3 客户池)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Lead.Dto;
using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Infrastructure.Repositories;

public interface ILeadRepository
{
    Task<LeadCustomer?> GetByIdAsync(long id, long tenantId);
    Task<PagedResult<LeadCustomer>> QueryAsync(long tenantId, LeadQueryRequest query);
    Task<LeadCustomer?> FindByPhoneAsync(long tenantId, string phone);
    Task<LeadCustomer?> FindByWeChatAsync(long tenantId, string weChat);
    Task<LeadCustomer> CreateAsync(LeadCustomer entity);
    Task<LeadCustomer> UpdateAsync(LeadCustomer entity);
    Task<List<LeadCustomer>> GetHotLeadsAsync(long tenantId, int top = 10);
    Task<int> CountByStatusAsync(long tenantId, string status);
}

public class LeadRepository : ILeadRepository
{
    private readonly AppDbContext _db;

    public LeadRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<LeadCustomer?> GetByIdAsync(long id, long tenantId)
    {
        return await _db.LeadCustomers
            .Include(l => l.AssignedUser)
            .FirstOrDefaultAsync(l => l.Id == id && l.TenantId == tenantId);
    }

    public async Task<PagedResult<LeadCustomer>> QueryAsync(long tenantId, LeadQueryRequest query)
    {
        var q = _db.LeadCustomers
            .Include(l => l.AssignedUser)
            .Where(l => l.TenantId == tenantId);

        if (!string.IsNullOrEmpty(query.Status))
            q = q.Where(l => l.Status == query.Status);
        if (!string.IsNullOrEmpty(query.SourcePlatform))
            q = q.Where(l => l.SourcePlatform == query.SourcePlatform);
        if (query.AssignedUserId.HasValue)
            q = q.Where(l => l.AssignedUserId == query.AssignedUserId);
        if (!string.IsNullOrEmpty(query.City))
            q = q.Where(l => l.City == query.City);
        if (!string.IsNullOrEmpty(query.Keyword))
            q = q.Where(l => (l.Name != null && l.Name.Contains(query.Keyword))
                          || (l.Phone != null && l.Phone.Contains(query.Keyword))
                          || (l.WeChat != null && l.WeChat.Contains(query.Keyword)));
        if (query.StartDate.HasValue)
            q = q.Where(l => l.CreatedAt >= query.StartDate.Value);
        if (query.EndDate.HasValue)
            q = q.Where(l => l.CreatedAt <= query.EndDate.Value);

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(l => l.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<LeadCustomer>
        {
            Items = items,
            Total = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<LeadCustomer?> FindByPhoneAsync(long tenantId, string phone)
    {
        return await _db.LeadCustomers
            .FirstOrDefaultAsync(l => l.TenantId == tenantId && l.Phone == phone);
    }

    public async Task<LeadCustomer?> FindByWeChatAsync(long tenantId, string weChat)
    {
        return await _db.LeadCustomers
            .FirstOrDefaultAsync(l => l.TenantId == tenantId && l.WeChat == weChat);
    }

    public async Task<LeadCustomer> CreateAsync(LeadCustomer entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        _db.LeadCustomers.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<LeadCustomer> UpdateAsync(LeadCustomer entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _db.LeadCustomers.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<List<LeadCustomer>> GetHotLeadsAsync(long tenantId, int top = 10)
    {
        return await _db.LeadCustomers
            .Include(l => l.AssignedUser)
            .Where(l => l.TenantId == tenantId && l.Status == "Hot")
            .OrderByDescending(l => l.UpdatedAt)
            .Take(top)
            .ToListAsync();
    }

    public async Task<int> CountByStatusAsync(long tenantId, string status)
    {
        return await _db.LeadCustomers
            .CountAsync(l => l.TenantId == tenantId && l.Status == status);
    }
}
