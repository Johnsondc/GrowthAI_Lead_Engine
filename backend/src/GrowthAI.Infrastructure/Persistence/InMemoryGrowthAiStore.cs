using GrowthAI.Domain.Entities;
using GrowthAI.Domain.Enums;

namespace GrowthAI.Infrastructure.Persistence;

public sealed class InMemoryGrowthAiStore
{
    public Guid DemoTenantId { get; } = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public List<LeadCustomer> Leads { get; } = [];

    public InMemoryGrowthAiStore()
    {
        Leads.AddRange([
            new LeadCustomer { TenantId = DemoTenantId, Name = "李女士", Phone = "13800000001", Wechat = "mama_cd_01", City = "成都", SourcePlatform = SourcePlatform.Douyin, SourceAccount = "成都月嫂避坑", InterestedProduct = "月嫂", ConsultationContent = "想了解预产期 9 月的月嫂价格", Status = LeadStatus.HighIntent },
            new LeadCustomer { TenantId = DemoTenantId, Name = "王先生", Phone = "13800000002", City = "成都", SourcePlatform = SourcePlatform.Xiaohongshu, SourceAccount = "新手妈妈指南", InterestedProduct = "产康", ConsultationContent = "产后恢复大概多少钱", Status = LeadStatus.Communicating },
            new LeadCustomer { TenantId = DemoTenantId, Name = "张女士", Wechat = "babycare_zhang", City = "重庆", SourcePlatform = SourcePlatform.H5Form, InterestedProduct = "育儿嫂", ConsultationContent = "需要住家育儿嫂", Status = LeadStatus.New }
        ]);
    }
}
