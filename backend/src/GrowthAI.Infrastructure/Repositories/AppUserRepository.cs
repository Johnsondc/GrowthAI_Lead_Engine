// ============================================
// 鍔熻兘鎻忚堪锛氳璇佷笌澶氱鎴锋ā鍧?- 鐢ㄦ埛鏁版嵁浠撳簱
// 鐢熸垚锛歈oder by 搴勫洯
// 鐢熸垚鏃ユ湡锛?026-07-21
// ============================================

using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Infrastructure.Repositories;

public interface IAppUserRepository
{
    Task<AppUser?> GetByIdAsync(long id);
    Task<AppUser?> GetByPhoneAsync(long tenantId, string phone);
    Task<List<AppUser>> GetByTenantAsync(long tenantId);
    Task<AppUser> CreateAsync(AppUser user);
    Task<AppUser> UpdateAsync(AppUser user);
    Task DeleteAsync(long id);
}

public class AppUserRepository : IAppUserRepository
{
    private readonly AppDbContext _db;

    public AppUserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AppUser?> GetByIdAsync(long id)
    {
        return await _db.AppUsers.Include(u => u.Tenant).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<AppUser?> GetByPhoneAsync(long tenantId, string phone)
    {
        return await _db.AppUsers
            .Include(u => u.Tenant)
            .FirstOrDefaultAsync(u => u.TenantId == tenantId && u.Phone == phone && u.Status == 1);
    }

    public async Task<List<AppUser>> GetByTenantAsync(long tenantId)
    {
        return await _db.AppUsers
            .Where(u => u.TenantId == tenantId)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<AppUser> CreateAsync(AppUser user)
    {
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        _db.AppUsers.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<AppUser> UpdateAsync(AppUser user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        _db.AppUsers.Update(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(long id)
    {
        var user = await _db.AppUsers.FindAsync(id);
        if (user != null)
        {
            user.Status = 0; // Soft delete
            await _db.SaveChangesAsync();
        }
    }
}