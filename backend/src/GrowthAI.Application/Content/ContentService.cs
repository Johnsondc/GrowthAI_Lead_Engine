// ============================================
// 功能描述：内容中心模块 - 内容服务
// Sprint: 8 (M4 内容中心)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Content.Dto;
using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Repositories;

namespace GrowthAI.Application.Content;

public interface IContentService
{
    Task<ContentPagedResult> QueryAsync(long tenantId, ContentQueryRequest query);
    Task<ContentDto?> GetByIdAsync(long tenantId, long id);
    Task<ContentDto> CreateAsync(long tenantId, CreateContentRequest request);
    Task<ContentDto?> UpdateAsync(long tenantId, long id, UpdateContentRequest request);
    Task<bool> DeleteAsync(long tenantId, long id);
    Task<Dictionary<string, int>> GetStatisticsAsync(long tenantId);
}

public class ContentService : IContentService
{
    private readonly IContentRepository _repo;
    private readonly ITenantRepository _tenantRepo;

    public ContentService(IContentRepository repo, ITenantRepository tenantRepo)
    {
        _repo = repo;
        _tenantRepo = tenantRepo;
    }

    public async Task<ContentPagedResult> QueryAsync(long tenantId, ContentQueryRequest query)
    {
        return await _repo.QueryAsync(tenantId, query);
    }

    public async Task<ContentDto?> GetByIdAsync(long tenantId, long id)
    {
        var entity = await _repo.GetByIdAsync(id, tenantId);
        if (entity == null) return null;

        return new ContentDto
        {
            Id = entity.Id,
            ContentType = entity.ContentType,
            TargetPlatform = entity.TargetPlatform,
            Title = entity.Title,
            Body = entity.Body,
            Tags = entity.Tags,
            Cta = entity.Cta,
            Industry = entity.Industry,
            Product = entity.Product,
            City = entity.City,
            AiTaskId = entity.AiTaskId,
            CreatedAt = entity.CreatedAt
        };
    }

    public async Task<ContentDto> CreateAsync(long tenantId, CreateContentRequest request)
    {
        // 获取租户行业信息
        var tenant = await _tenantRepo.GetByIdAsync(tenantId);

        var entity = new AiContent
        {
            TenantId = tenantId,
            ContentType = request.ContentType,
            TargetPlatform = request.TargetPlatform,
            Title = request.Title,
            Body = request.Body,
            Tags = request.Tags,
            Cta = request.Cta,
            Industry = tenant?.Industry,
            Product = request.Product,
            City = request.City,
            Status = 1
        };

        var created = await _repo.CreateAsync(entity);
        return new ContentDto
        {
            Id = created.Id,
            ContentType = created.ContentType,
            TargetPlatform = created.TargetPlatform,
            Title = created.Title,
            Body = created.Body,
            Tags = created.Tags,
            Cta = created.Cta,
            Industry = created.Industry,
            Product = created.Product,
            City = created.City,
            AiTaskId = created.AiTaskId,
            CreatedAt = created.CreatedAt
        };
    }

    public async Task<ContentDto?> UpdateAsync(long tenantId, long id, UpdateContentRequest request)
    {
        var entity = await _repo.GetByIdAsync(id, tenantId);
        if (entity == null) return null;

        if (request.Title != null) entity.Title = request.Title;
        if (request.Body != null) entity.Body = request.Body;
        if (request.Tags != null) entity.Tags = request.Tags;
        if (request.Cta != null) entity.Cta = request.Cta;
        if (request.TargetPlatform != null) entity.TargetPlatform = request.TargetPlatform;

        var updated = await _repo.UpdateAsync(entity);
        return new ContentDto
        {
            Id = updated.Id,
            ContentType = updated.ContentType,
            TargetPlatform = updated.TargetPlatform,
            Title = updated.Title,
            Body = updated.Body,
            Tags = updated.Tags,
            Cta = updated.Cta,
            Industry = updated.Industry,
            Product = updated.Product,
            City = updated.City,
            AiTaskId = updated.AiTaskId,
            CreatedAt = updated.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(long tenantId, long id)
    {
        var entity = await _repo.GetByIdAsync(id, tenantId);
        if (entity == null) return false;

        await _repo.DeleteAsync(id);
        return true;
    }

    public async Task<Dictionary<string, int>> GetStatisticsAsync(long tenantId)
    {
        return await _repo.GetStatisticsAsync(tenantId);
    }
}
