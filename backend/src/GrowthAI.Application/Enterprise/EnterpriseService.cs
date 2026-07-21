// ============================================
// 功能描述：企业管理模块 - 企业服务
// Sprint: 5 (M2 企业管理)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Enterprise.Dto;
using GrowthAI.Infrastructure.Repositories;

namespace GrowthAI.Application.Enterprise;

public interface IEnterpriseService
{
    Task<EnterpriseInfoDto?> GetEnterpriseAsync(long tenantId);
    Task<EnterpriseInfoDto?> UpdateEnterpriseAsync(long tenantId, UpdateEnterpriseRequest request);
}

public class EnterpriseService : IEnterpriseService
{
    private readonly ITenantRepository _tenantRepo;

    public EnterpriseService(ITenantRepository tenantRepo)
    {
        _tenantRepo = tenantRepo;
    }

    public async Task<EnterpriseInfoDto?> GetEnterpriseAsync(long tenantId)
    {
        var tenant = await _tenantRepo.GetByIdAsync(tenantId);
        if (tenant == null) return null;

        return new EnterpriseInfoDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Industry = tenant.Industry,
            ContactPhone = tenant.ContactPhone,
            PlanType = tenant.PlanType,
            Status = tenant.Status
        };
    }

    public async Task<EnterpriseInfoDto?> UpdateEnterpriseAsync(long tenantId, UpdateEnterpriseRequest request)
    {
        var tenant = await _tenantRepo.GetByIdAsync(tenantId);
        if (tenant == null) return null;

        if (!string.IsNullOrEmpty(request.Name))
            tenant.Name = request.Name;
        if (request.Industry != null)
            tenant.Industry = request.Industry;
        if (request.ContactPhone != null)
            tenant.ContactPhone = request.ContactPhone;

        await _tenantRepo.UpdateAsync(tenant);

        return new EnterpriseInfoDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Industry = tenant.Industry,
            ContactPhone = tenant.ContactPhone,
            PlanType = tenant.PlanType,
            Status = tenant.Status
        };
    }
}
