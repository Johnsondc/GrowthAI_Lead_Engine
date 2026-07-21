// ============================================
// 功能描述：客户池模块 - 客户服务
// Sprint: 6 (M3 客户池)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Lead.Dto;
using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Repositories;

namespace GrowthAI.Application.Lead;

public interface ILeadService
{
    Task<PagedResult<LeadCustomerDto>> QueryAsync(long tenantId, LeadQueryRequest query);
    Task<LeadCustomerDto?> GetByIdAsync(long tenantId, long id);
    Task<LeadCustomerDto> CreateAsync(long tenantId, long? currentUserId, CreateLeadCustomerRequest request);
    Task<LeadCustomerDto?> UpdateAsync(long tenantId, long id, UpdateLeadCustomerRequest request);
    Task<LeadCustomerDto?> UpdateStatusAsync(long tenantId, long id, UpdateLeadStatusRequest request);
    Task<LeadCustomerDto?> AssignAsync(long tenantId, long id, AssignLeadRequest request);
    Task<List<FollowUpRecordDto>> GetFollowUpsAsync(long tenantId, long leadId);
    Task<FollowUpRecordDto> CreateFollowUpAsync(long tenantId, long userId, long leadId, CreateFollowUpRequest request);
    Task<List<LeadCustomerDto>> GetHotLeadsAsync(long tenantId, int top = 10);
    Task<Dictionary<string, int>> GetStatisticsAsync(long tenantId);
}

public class LeadService : ILeadService
{
    private readonly ILeadRepository _leadRepo;
    private readonly IFollowUpRecordRepository _followUpRepo;
    private readonly IAppUserRepository _userRepo;

    // 状态流转规则: New->Contacted->InProgress->Hot/Closed/Invalid
    private static readonly Dictionary<string, string[]> ValidTransitions = new()
    {
        ["New"] = new[] { "Contacted", "Invalid" },
        ["Contacted"] = new[] { "InProgress", "Invalid" },
        ["InProgress"] = new[] { "Hot", "Closed", "Invalid" },
        ["Hot"] = new[] { "Closed", "Invalid" },
        ["Closed"] = Array.Empty<string>(),
        ["Invalid"] = Array.Empty<string>()
    };

    public LeadService(
        ILeadRepository leadRepo,
        IFollowUpRecordRepository followUpRepo,
        IAppUserRepository userRepo)
    {
        _leadRepo = leadRepo;
        _followUpRepo = followUpRepo;
        _userRepo = userRepo;
    }

    public async Task<PagedResult<LeadCustomerDto>> QueryAsync(long tenantId, LeadQueryRequest query)
    {
        var result = await _leadRepo.QueryAsync(tenantId, query);
        return new PagedResult<LeadCustomerDto>
        {
            Items = result.Items.Select(MapToDto).ToList(),
            Total = result.Total,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }

    public async Task<LeadCustomerDto?> GetByIdAsync(long tenantId, long id)
    {
        var entity = await _leadRepo.GetByIdAsync(id, tenantId);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<LeadCustomerDto> CreateAsync(long tenantId, long? currentUserId, CreateLeadCustomerRequest request)
    {
        // 去重检测: 手机号
        if (!string.IsNullOrEmpty(request.Phone))
        {
            var existing = await _leadRepo.FindByPhoneAsync(tenantId, request.Phone);
            if (existing != null)
                throw new InvalidOperationException("该手机号已存在客户记录");
        }

        // 去重检测: 微信号
        if (!string.IsNullOrEmpty(request.WeChat))
        {
            var existing = await _leadRepo.FindByWeChatAsync(tenantId, request.WeChat);
            if (existing != null)
                throw new InvalidOperationException("该微信号已存在客户记录");
        }

        var entity = new LeadCustomer
        {
            TenantId = tenantId,
            Name = request.Name,
            Phone = request.Phone,
            WeChat = request.WeChat,
            City = request.City,
            SourcePlatform = request.SourcePlatform,
            SourceAccount = request.SourceAccount,
            ConsultContent = request.ConsultContent,
            InterestProduct = request.InterestProduct,
            Remark = request.Remark,
            Tags = request.Tags,
            AssignedUserId = request.AssignedUserId ?? currentUserId,
            Status = "New",
            SourceType = "ManualInput"
        };

        var created = await _leadRepo.CreateAsync(entity);
        return MapToDto(created);
    }

    public async Task<LeadCustomerDto?> UpdateAsync(long tenantId, long id, UpdateLeadCustomerRequest request)
    {
        var entity = await _leadRepo.GetByIdAsync(id, tenantId);
        if (entity == null) return null;

        if (request.Name != null) entity.Name = request.Name;
        if (request.Phone != null) entity.Phone = request.Phone;
        if (request.WeChat != null) entity.WeChat = request.WeChat;
        if (request.City != null) entity.City = request.City;
        if (request.SourcePlatform != null) entity.SourcePlatform = request.SourcePlatform;
        if (request.InterestProduct != null) entity.InterestProduct = request.InterestProduct;
        if (request.Remark != null) entity.Remark = request.Remark;
        if (request.Tags != null) entity.Tags = request.Tags;

        var updated = await _leadRepo.UpdateAsync(entity);
        return MapToDto(updated);
    }

    public async Task<LeadCustomerDto?> UpdateStatusAsync(long tenantId, long id, UpdateLeadStatusRequest request)
    {
        var entity = await _leadRepo.GetByIdAsync(id, tenantId);
        if (entity == null) return null;

        // 状态流转校验
        if (!ValidTransitions.TryGetValue(entity.Status, out var allowed)
            || !allowed.Contains(request.Status))
        {
            throw new InvalidOperationException($"不允许从 {entity.Status} 变更为 {request.Status}");
        }

        entity.Status = request.Status;

        // 无效状态需要填写原因
        if (request.Status == "Invalid")
        {
            entity.InvalidReason = request.InvalidReason ?? "未填写";
        }

        var updated = await _leadRepo.UpdateAsync(entity);
        return MapToDto(updated);
    }

    public async Task<LeadCustomerDto?> AssignAsync(long tenantId, long id, AssignLeadRequest request)
    {
        var entity = await _leadRepo.GetByIdAsync(id, tenantId);
        if (entity == null) return null;

        // 验证目标用户存在
        var user = await _userRepo.GetByIdAsync(request.AssignedUserId);
        if (user == null || user.TenantId != tenantId)
            throw new InvalidOperationException("目标用户不存在");

        entity.AssignedUserId = request.AssignedUserId;
        var updated = await _leadRepo.UpdateAsync(entity);
        return MapToDto(updated);
    }

    public async Task<List<FollowUpRecordDto>> GetFollowUpsAsync(long tenantId, long leadId)
    {
        // 确认客户属于当前租户
        var lead = await _leadRepo.GetByIdAsync(leadId, tenantId);
        if (lead == null) return new List<FollowUpRecordDto>();

        var records = await _followUpRepo.GetByLeadAsync(leadId);
        return records.Select(r => new FollowUpRecordDto
        {
            Id = r.Id,
            FollowType = r.FollowType,
            Content = r.Content,
            FollowerName = r.Follower?.Name ?? "未知",
            NextFollowUpTime = r.NextFollowUpTime,
            CreatedAt = r.CreatedAt
        }).ToList();
    }

    public async Task<FollowUpRecordDto> CreateFollowUpAsync(long tenantId, long userId, long leadId, CreateFollowUpRequest request)
    {
        var lead = await _leadRepo.GetByIdAsync(leadId, tenantId);
        if (lead == null)
            throw new InvalidOperationException("客户不存在");

        var record = new FollowUpRecord
        {
            TenantId = tenantId,
            LeadCustomerId = leadId,
            FollowerId = userId,
            FollowType = request.FollowType,
            Content = request.Content,
            NextFollowUpTime = request.NextFollowUpTime
        };

        var created = await _followUpRepo.CreateAsync(record);

        // 更新客户的最后跟进时间
        lead.LastFollowUpTime = DateTime.UtcNow;
        await _leadRepo.UpdateAsync(lead);

        return new FollowUpRecordDto
        {
            Id = created.Id,
            FollowType = created.FollowType,
            Content = created.Content,
            FollowerName = string.Empty,
            NextFollowUpTime = created.NextFollowUpTime,
            CreatedAt = created.CreatedAt
        };
    }

    public async Task<List<LeadCustomerDto>> GetHotLeadsAsync(long tenantId, int top = 10)
    {
        var leads = await _leadRepo.GetHotLeadsAsync(tenantId, top);
        return leads.Select(MapToDto).ToList();
    }

    public async Task<Dictionary<string, int>> GetStatisticsAsync(long tenantId)
    {
        var stats = new Dictionary<string, int>();
        var statuses = new[] { "New", "Contacted", "InProgress", "Hot", "Closed", "Invalid" };
        foreach (var status in statuses)
        {
            stats[status] = await _leadRepo.CountByStatusAsync(tenantId, status);
        }
        stats["Total"] = stats.Values.Sum();
        return stats;
    }

    private static LeadCustomerDto MapToDto(LeadCustomer entity)
    {
        return new LeadCustomerDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Phone = entity.Phone,
            WeChat = entity.WeChat,
            City = entity.City,
            SourcePlatform = entity.SourcePlatform,
            SourceAccount = entity.SourceAccount,
            ConsultContent = entity.ConsultContent,
            InterestProduct = entity.InterestProduct,
            Status = entity.Status,
            SourceType = entity.SourceType,
            Remark = entity.Remark,
            Tags = entity.Tags,
            AssignedUserId = entity.AssignedUserId,
            AssignedUserName = entity.AssignedUser?.Name,
            LastFollowUpTime = entity.LastFollowUpTime,
            InvalidReason = entity.InvalidReason,
            CreatedAt = entity.CreatedAt
        };
    }
}
