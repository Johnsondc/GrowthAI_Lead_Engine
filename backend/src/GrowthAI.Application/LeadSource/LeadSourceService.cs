// ============================================
// 功能描述：来源管理模块 - 来源服务
// Sprint: 7 (M7 来源管理)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.LeadSource.Dto;
using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Repositories;

namespace GrowthAI.Application.LeadSource;

public interface ILeadSourceService
{
    Task<List<LeadSourceDto>> GetAllAsync(long tenantId);
    Task<LeadSourceDto?> GetByIdAsync(long tenantId, long id);
    Task<LeadSourceDto> CreateAsync(long tenantId, CreateLeadSourceRequest request);
    Task<LeadSourceDto?> UpdateAsync(long tenantId, long id, UpdateLeadSourceRequest request);
    Task<bool> DeleteAsync(long tenantId, long id);
    Task<LeadSourceDto?> GetByTrackingCodeAsync(string trackingCode);
}

public class LeadSourceService : ILeadSourceService
{
    private readonly ILeadSourceRepository _repo;

    public LeadSourceService(ILeadSourceRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<LeadSourceDto>> GetAllAsync(long tenantId)
    {
        var sources = await _repo.GetByTenantAsync(tenantId);
        return sources.Select(MapToDto).ToList();
    }

    public async Task<LeadSourceDto?> GetByIdAsync(long tenantId, long id)
    {
        var entity = await _repo.GetByIdAsync(id, tenantId);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<LeadSourceDto> CreateAsync(long tenantId, CreateLeadSourceRequest request)
    {
        var entity = new LeadSource
        {
            TenantId = tenantId,
            SourceType = request.SourceType,
            Platform = request.Platform,
            AccountName = request.AccountName,
            LandingPageId = request.LandingPageId,
            TrackingCode = GenerateTrackingCode()
        };

        var created = await _repo.CreateAsync(entity);
        return MapToDto(created);
    }

    public async Task<LeadSourceDto?> UpdateAsync(long tenantId, long id, UpdateLeadSourceRequest request)
    {
        var entity = await _repo.GetByIdAsync(id, tenantId);
        if (entity == null) return null;

        if (request.SourceType != null) entity.SourceType = request.SourceType;
        if (request.Platform != null) entity.Platform = request.Platform;
        if (request.AccountName != null) entity.AccountName = request.AccountName;
        if (request.LandingPageId.HasValue) entity.LandingPageId = request.LandingPageId;

        var updated = await _repo.UpdateAsync(entity);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(long tenantId, long id)
    {
        var entity = await _repo.GetByIdAsync(id, tenantId);
        if (entity == null) return false;

        await _repo.DeleteAsync(id);
        return true;
    }

    public async Task<LeadSourceDto?> GetByTrackingCodeAsync(string trackingCode)
    {
        var entity = await _repo.GetByTrackingCodeAsync(trackingCode);
        return entity == null ? null : MapToDto(entity);
    }

    private static string GenerateTrackingCode()
    {
        return $"SRC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
    }

    private static LeadSourceDto MapToDto(LeadSource entity)
    {
        return new LeadSourceDto
        {
            Id = entity.Id,
            SourceType = entity.SourceType,
            Platform = entity.Platform,
            AccountName = entity.AccountName,
            TrackingCode = entity.TrackingCode,
            LandingPageId = entity.LandingPageId,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt
        };
    }
}
