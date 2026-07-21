// ============================================
// 功能描述：企业管理模块 - 用户管理服务
// Sprint: 5 (M2 企业管理)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Enterprise.Dto;
using GrowthAI.Domain.Entities;
using GrowthAI.Domain.Enums;
using GrowthAI.Infrastructure.Repositories;

namespace GrowthAI.Application.Enterprise;

public interface IAppUserService
{
    Task<List<AppUserDto>> GetUsersAsync(long tenantId);
    Task<AppUserDto?> GetUserByIdAsync(long tenantId, long userId);
    Task<AppUserDto?> CreateUserAsync(long tenantId, CreateAppUserRequest request);
    Task<AppUserDto?> UpdateUserAsync(long tenantId, long userId, UpdateAppUserRequest request);
    Task<bool> DeleteUserAsync(long tenantId, long userId);
}

public class AppUserService : IAppUserService
{
    private readonly IAppUserRepository _userRepo;

    public AppUserService(IAppUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<List<AppUserDto>> GetUsersAsync(long tenantId)
    {
        var users = await _userRepo.GetByTenantAsync(tenantId);
        return users.Select(MapToDto).ToList();
    }

    public async Task<AppUserDto?> GetUserByIdAsync(long tenantId, long userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null || user.TenantId != tenantId) return null;
        return MapToDto(user);
    }

    public async Task<AppUserDto?> CreateUserAsync(long tenantId, CreateAppUserRequest request)
    {
        // 检查手机号是否已存在
        var existing = await _userRepo.GetByPhoneAsync(tenantId, request.Phone);
        if (existing != null) return null;

        if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
            role = UserRole.Operator;

        var user = new AppUser
        {
            TenantId = tenantId,
            Name = request.Name,
            Phone = request.Phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, BCrypt.Net.BCrypt.GenerateSalt(11)),
            Role = role,
            Status = 1
        };

        user = await _userRepo.CreateAsync(user);
        return MapToDto(user);
    }

    public async Task<AppUserDto?> UpdateUserAsync(long tenantId, long userId, UpdateAppUserRequest request)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null || user.TenantId != tenantId) return null;

        if (!string.IsNullOrEmpty(request.Name))
            user.Name = request.Name;
        if (!string.IsNullOrEmpty(request.Phone))
        {
            // 检查新手机号是否已被其他人使用
            var existing = await _userRepo.GetByPhoneAsync(tenantId, request.Phone);
            if (existing != null && existing.Id != userId) return null;
            user.Phone = request.Phone;
        }
        if (!string.IsNullOrEmpty(request.Role) && Enum.TryParse<UserRole>(request.Role, true, out var role))
            user.Role = role;
        if (request.Status.HasValue)
            user.Status = request.Status.Value;

        await _userRepo.UpdateAsync(user);
        return MapToDto(user);
    }

    public async Task<bool> DeleteUserAsync(long tenantId, long userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null || user.TenantId != tenantId) return false;
        await _userRepo.DeleteAsync(userId);
        return true;
    }

    private static AppUserDto MapToDto(AppUser user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Phone = user.Phone,
        Role = user.Role.ToString(),
        Status = user.Status,
        CreatedAt = user.CreatedAt
    };
}
