using GrowthAI.Application.Ai;

namespace GrowthAI.Infrastructure.Persistence;

public sealed class AiContentService : IAiContentService
{
    public GeneratedContentDto Generate(GenerateContentRequest request)
    {
        var title = $"{request.City}{request.ProductName}怎么选？给{request.TargetAudience}的避坑指南";
        var body = $"围绕{request.Industry}行业，突出{request.SellingPoints}，引导用户评论关键词或进入表单领取方案。";
        var script = $"开头3秒：还在纠结{request.ProductName}？\n痛点：很多{request.TargetAudience}不知道怎么判断服务是否靠谱。\n方案：用3个标准筛选，并领取本地匹配方案。";
        var tags = $"#{request.City} #{request.ProductName} #{request.Industry} #新媒体获客";
        var cta = "评论区回复关键词，或扫码填写姓名、电话、微信和需求，顾问将尽快联系。";

        return new GeneratedContentDto(request.Platform, title, body, script, tags, cta);
    }
}
