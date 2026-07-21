// ============================================
// 功能描述：引流页模块 - 服务
// Sprint: 9 (M6 引流页)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.LandingPage.Dto;
using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Repositories;

namespace GrowthAI.Application.LandingPage;

public interface ILandingPageService
{
    Task<List<LandingPageDto>> GetAllAsync(long tenantId);
    Task<LandingPageDto?> GetByIdAsync(long tenantId, long id);
    Task<LandingPageDto> CreateAsync(long tenantId, CreateLandingPageRequest request);
    Task<LandingPageDto?> UpdateAsync(long tenantId, long id, UpdateLandingPageRequest request);
    Task<bool> DeleteAsync(long tenantId, long id);
    Task<LandingPageDto?> GetPublicPageAsync(string pageCode);
    Task RecordViewAsync(string pageCode);
    Task<bool> SubmitFormAsync(long tenantId, long pageId, LandingPageSubmitRequest request);
    Task<List<LandingPageStatsDto>> GetStatisticsAsync(long tenantId);
}

public class LandingPageService : ILandingPageService
{
    private readonly ILandingPageRepository _pageRepo;
    private readonly ILeadRepository _leadRepo;

    public LandingPageService(ILandingPageRepository pageRepo, ILeadRepository leadRepo)
    {
        _pageRepo = pageRepo;
        _leadRepo = leadRepo;
    }

    public async Task<List<LandingPageDto>> GetAllAsync(long tenantId)
    {
        var pages = await _pageRepo.GetByTenantAsync(tenantId);
        return pages.Select(MapToDto).ToList();
    }

    public async Task<LandingPageDto?> GetByIdAsync(long tenantId, long id)
    {
        var entity = await _pageRepo.GetByIdAsync(id, tenantId);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<LandingPageDto> CreateAsync(long tenantId, CreateLandingPageRequest request)
    {
        var entity = new LandingPage
        {
            TenantId = tenantId,
            Title = request.Title,
            Description = request.Description,
            CoverImage = request.CoverImage,
            FormConfig = request.FormConfig,
            CustomCss = request.CustomCss,
            ThankYouMessage = request.ThankYouMessage ?? "感谢您的咨询，我们将尽快联系您！",
            RedirectUrl = request.RedirectUrl,
            PageCode = GeneratePageCode()
        };

        var created = await _pageRepo.CreateAsync(entity);
        return MapToDto(created);
    }

    public async Task<LandingPageDto?> UpdateAsync(long tenantId, long id, UpdateLandingPageRequest request)
    {
        var entity = await _pageRepo.GetByIdAsync(id, tenantId);
        if (entity == null) return null;

        if (request.Title != null) entity.Title = request.Title;
        if (request.Description != null) entity.Description = request.Description;
        if (request.CoverImage != null) entity.CoverImage = request.CoverImage;
        if (request.FormConfig != null) entity.FormConfig = request.FormConfig;
        if (request.CustomCss != null) entity.CustomCss = request.CustomCss;
        if (request.ThankYouMessage != null) entity.ThankYouMessage = request.ThankYouMessage;
        if (request.RedirectUrl != null) entity.RedirectUrl = request.RedirectUrl;
        if (request.Status.HasValue) entity.Status = request.Status.Value;

        var updated = await _pageRepo.UpdateAsync(entity);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(long tenantId, long id)
    {
        var entity = await _pageRepo.GetByIdAsync(id, tenantId);
        if (entity == null) return false;

        await _pageRepo.DeleteAsync(id);
        return true;
    }

    public async Task<LandingPageDto?> GetPublicPageAsync(string pageCode)
    {
        var entity = await _pageRepo.GetByPageCodeAsync(pageCode);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task RecordViewAsync(string pageCode)
    {
        var entity = await _pageRepo.GetByPageCodeAsync(pageCode);
        if (entity != null)
        {
            await _pageRepo.IncrementViewCountAsync(entity.Id);
        }
    }

    public async Task<bool> SubmitFormAsync(long tenantId, long pageId, LandingPageSubmitRequest request)
    {
        var page = await _pageRepo.GetByIdAsync(pageId, tenantId);
        if (page == null) return false;

        // 创建客户线索
        var lead = new LeadCustomer
        {
            TenantId = tenantId,
            Name = request.Name,
            Phone = request.Phone,
            WeChat = request.WeChat,
            City = request.City,
            ConsultContent = request.ConsultContent,
            SourceType = "LandingForm",
            Status = "New"
        };

        await _leadRepo.CreateAsync(lead);
        await _pageRepo.IncrementSubmitCountAsync(pageId);
        return true;
    }

    public async Task<List<LandingPageStatsDto>> GetStatisticsAsync(long tenantId)
    {
        var pages = await _pageRepo.GetByTenantAsync(tenantId);
        return pages.Select(p => new LandingPageStatsDto
        {
            PageId = p.Id,
            Title = p.Title,
            ViewCount = p.ViewCount,
            SubmitCount = p.SubmitCount
        }).ToList();
    }

    private static string GeneratePageCode()
    {
        return $"lp-{Guid.NewGuid().ToString("N")[..10]}";
    }

    private static LandingPageDto MapToDto(LandingPage entity)
    {
        return new LandingPageDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            CoverImage = entity.CoverImage,
            PageCode = entity.PageCode,
            FormConfig = entity.FormConfig,
            CustomCss = entity.CustomCss,
            ThankYouMessage = entity.ThankYouMessage,
            RedirectUrl = entity.RedirectUrl,
            ViewCount = entity.ViewCount,
            SubmitCount = entity.SubmitCount,
            Status = entity.Status,
            AccessUrl = $"/page/{entity.PageCode}",
            CreatedAt = entity.CreatedAt
        };
    }
}
