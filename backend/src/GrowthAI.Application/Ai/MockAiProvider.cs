// ============================================
// 功能描述：AI引擎模块 - Mock AI Provider（V1阶段使用）
// Sprint: 4 (M5 AI引擎)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using System.Diagnostics;

namespace GrowthAI.Application.Ai;

/// <summary>
/// Mock AI Provider - V1阶段使用，返回模拟数据
/// 后续可替换为真实的 AI Provider（OpenAI / 通义千问 / 文心等）
/// </summary>
public class MockAiProvider : IAiProvider
{
    public async Task<AiProviderResult> SendMessageAsync(string systemPrompt, string userMessage)
    {
        var sw = Stopwatch.StartNew();

        // 模拟AI处理延迟（200-800ms）
        await Task.Delay(Random.Shared.Next(200, 800));

        sw.Stop();

        // 根据提示词内容生成不同类型的模拟结果
        var content = GenerateMockContent(systemPrompt, userMessage);

        return new AiProviderResult
        {
            Success = true,
            Content = content,
            TokensUsed = Random.Shared.Next(100, 500),
            DurationMs = (int)sw.ElapsedMilliseconds
        };
    }

    private string GenerateMockContent(string systemPrompt, string userMessage)
    {
        // 根据系统提示词中的关键词判断内容类型
        if (systemPrompt.Contains("短视频") || systemPrompt.Contains("脚本"))
        {
            return GenerateMockVideoScript(userMessage);
        }
        else if (systemPrompt.Contains("朋友圈") || systemPrompt.Contains("文案"))
        {
            return GenerateMockMomentPost(userMessage);
        }
        else if (systemPrompt.Contains("评分") || systemPrompt.Contains("匹配"))
        {
            return GenerateMockScore(userMessage);
        }
        else
        {
            return GenerateMockImageTextPost(userMessage);
        }
    }

    private string GenerateMockImageTextPost(string input)
    {
        return """
        {
            "title": "【母婴好物】新手妈妈必备的10件神器，第5件99%的人都买错了！",
            "body": "姐妹们！作为一个带了两娃的宝妈，今天必须给大家分享我的血泪经验...\n\n1. 恒温水壶 - 夜奶救星\n2. 婴儿监护器 - 安心睡眠\n3. 辅食机 - 6月龄必备\n4. 尿布台 - 保护腰椎\n5. 安全座椅 - 出行刚需\n\n每一件都是我踩过无数坑后总结出来的，特别是第5件，选错了真的很危险！\n\n点击链接免费领取试用装 👇",
            "tags": ["母婴好物", "新手妈妈", "育儿经验", "宝宝用品", "种草推荐"],
            "cta": "点击下方链接，免费领取母婴试用装大礼包！名额有限，先到先得~"
        }
        """;
    }

    private string GenerateMockVideoScript(string input)
    {
        return """
        {
            "title": "宝妈必看！这3个育儿误区害了多少孩子",
            "body": "【开场】(0-3秒)\n画面：宝妈抱着宝宝，表情困惑\n旁白：你是不是也这样带娃？\n\n【痛点】(3-10秒)\n画面：错误示范场景\n旁白：很多新手妈妈都不知道，这3个常见做法其实会伤害宝宝...\n\n【干货】(10-45秒)\n画面：正确示范+文字标注\n旁白：第一个误区... 正确做法应该是...\n\n【引导】(45-60秒)\n画面：产品特写+二维码\n旁白：想要更多育儿干货？关注我，免费领取专业指导！",
            "tags": ["育儿误区", "新手妈妈", "母婴知识"],
            "cta": "关注@XX母婴，每天学一个育儿小知识！私信'领取'获取免费育儿手册"
        }
        """;
    }

    private string GenerateMockMomentPost(string input)
    {
        return """
        {
            "title": "",
            "body": "今天又收到一位宝妈的好评反馈🥰\n她说用了我们推荐的产品后，宝宝睡眠好了很多～\n其实选对产品真的很重要，不用贵的，只要对的💕\n有需要的姐妹可以私聊我，帮你分析宝宝的情况哦～",
            "tags": [],
            "cta": "评论区扣1，免费帮你分析宝宝睡眠问题～"
        }
        """;
    }

    private string GenerateMockScore(string input)
    {
        var score = Random.Shared.Next(60, 98);
        return $$"""
        {
            "score": {{score}},
            "reason": "该用户在母婴相关话题下有较高的活跃度和互动率，近期发布了多条育儿相关内容，符合目标客户画像",
            "confidence": 0.85
        }
        """;
    }
}
