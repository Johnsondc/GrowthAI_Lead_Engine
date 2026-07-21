-- ============================================
-- GrowthAI Lead Engine - AI Engine Migration
-- Sprint 4: M5 AI引擎 - Prompt模板表 + 种子数据
-- 生成：Qoder by 庄园
-- 生成日期：2026-07-21
-- ============================================

USE growthai_dev;

-- =============================================
-- AI Prompt模板表
-- =============================================
CREATE TABLE IF NOT EXISTS AiPromptTemplate (
    Id BIGINT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL COMMENT '模板名称',
    Industry VARCHAR(50) NOT NULL COMMENT '行业',
    Platform VARCHAR(30) NOT NULL COMMENT '平台: 抖音/小红书/视频号/朋友圈',
    ContentType VARCHAR(30) NOT NULL COMMENT '内容类型: ImageText/ShortVideoScript/MomentPost',
    PromptText TEXT NOT NULL COMMENT 'Prompt提示词内容',
    Variables JSON COMMENT '变量定义(JSON)',
    Status TINYINT DEFAULT 1 COMMENT '1=启用 0=停用',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_prompt_match (Industry, Platform, ContentType)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='AI Prompt模板';

-- =============================================
-- 种子数据：母婴行业 - 4个平台各一套Prompt
-- =============================================
INSERT INTO AiPromptTemplate (Name, Industry, Platform, ContentType, PromptText, Variables) VALUES
(
    '母婴-小红书-图文种草',
    '母婴', '小红书', 'ImageText',
    '你是一位专业的小红书母婴内容创作师。请根据以下信息，为母婴产品创作一篇种草风格的图文内容。\n\n要求：\n1. 标题不超过20字，要有吸引力和emoji\n2. 正文不超过1000字，要有真实感和分享感\n3. 标签不超过10个，要精准匹配母婴人群\n4. CTA要有引导性，促进互动\n\n请以JSON格式返回，包含title、body、tags(数组)、cta四个字段。',
    '["industry","product","city","targetCustomer","sellingPoint"]'
),
(
    '母婴-抖音-短视频脚本',
    '母婴', '抖音', 'ShortVideoScript',
    '你是一位专业的抖音短视频脚本撰写师，擅长为母婴品牌创作种草类短视频脚本。\n\n要求：\n1. 标题不超过20字\n2. 脚本按场景拆分，总时长控制在60秒内\n3. 包含开场hook、痛点、产品展示、引导关注\n4. 标签不超过5个\n\n请以JSON格式返回，包含title、body（含分镜建议）、tags(数组)、cta四个字段。',
    '["industry","product","city","targetCustomer","sellingPoint"]'
),
(
    '母婴-视频号-正式内容',
    '母婴', '视频号', 'ImageText',
    '你是一位专业的视频号内容创作师，风格偏正式、专业，适合母婴行业的知识类内容。\n\n要求：\n1. 标题不超过30字\n2. 正文不超过500字，要有专业性和权威感\n3. 包含引导关注的话术\n4. 标签不超过5个\n\n请以JSON格式返回，包含title、body、tags(数组)、cta四个字段。',
    '["industry","product","city","targetCustomer","sellingPoint"]'
),
(
    '母婴-朋友圈-文案',
    '母婴', '朋友圈', 'MomentPost',
    '你是一位朋友圈文案专家，擅长为母婴品牌撰写口语化、有互动引导的朋友圈文案。\n\n要求：\n1. 不需要标题\n2. 正文不超过200字\n3. 口语化，像朋友分享\n4. 包含互动引导\n\n请以JSON格式返回，包含title(空字符串)、body、tags(空数组)、cta四个字段。',
    '["industry","product","city","targetCustomer","sellingPoint"]'
),
(
    '母婴-通用-匹配度评分',
    '母婴', '通用', 'MatchScoring',
    '你是一位AI客户线索分析师。请根据以下潜在客户信息，评估其与母婴行业的匹配度。\n\n评分维度：\n1. 内容相关性（是否涉及母婴话题）\n2. 活跃度（发布频率和互动率）\n3. 消费意愿（是否有购买意向的信号）\n\n请给出0-100的评分，并以JSON格式返回score、reason、confidence三个字段。',
    '["platform","content","profile"]'
);
