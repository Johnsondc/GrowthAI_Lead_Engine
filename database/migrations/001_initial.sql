-- ============================================
-- GrowthAI Lead Engine - Initial Schema Migration
-- Sprint 3: 创建6张核心业务表
-- 生成：Qoder by 庄园
-- 生成日期：2026-07-21
-- ============================================

CREATE DATABASE IF NOT EXISTS growthai_dev
    DEFAULT CHARACTER SET utf8mb4
    DEFAULT COLLATE utf8mb4_unicode_ci;

USE growthai_dev;

-- =============================================
-- 1. 租户/企业表
-- =============================================
CREATE TABLE IF NOT EXISTS Tenant (
    Id BIGINT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL COMMENT '企业名称',
    Industry VARCHAR(50) COMMENT '行业',
    ContactPhone VARCHAR(20) COMMENT '联系电话',
    PlanType VARCHAR(20) DEFAULT 'Basic' COMMENT '套餐类型: Basic/Pro/Enterprise',
    Status TINYINT DEFAULT 1 COMMENT '1=启用 0=停用',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_tenant_status (Status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='租户/企业';

-- =============================================
-- 2. 系统用户表
-- =============================================
CREATE TABLE IF NOT EXISTS AppUser (
    Id BIGINT AUTO_INCREMENT PRIMARY KEY,
    TenantId BIGINT NOT NULL COMMENT '所属租户',
    Name VARCHAR(50) NOT NULL COMMENT '姓名',
    Phone VARCHAR(20) NOT NULL COMMENT '手机号(登录账号)',
    PasswordHash VARCHAR(255) NOT NULL COMMENT '密码哈希',
    Role VARCHAR(20) NOT NULL DEFAULT 'Operator' COMMENT '角色: Admin/Owner/Operator/Sales',
    Status TINYINT DEFAULT 1 COMMENT '1=启用 0=停用',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_appuser_tenant (TenantId),
    INDEX idx_appuser_phone (Phone),
    UNIQUE KEY uk_tenant_phone (TenantId, Phone),
    FOREIGN KEY (TenantId) REFERENCES Tenant(Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='系统用户';

-- =============================================
-- 3. 客户来源表
-- =============================================
CREATE TABLE IF NOT EXISTS LeadSource (
    Id BIGINT AUTO_INCREMENT PRIMARY KEY,
    TenantId BIGINT NOT NULL COMMENT '所属租户',
    SourceType VARCHAR(30) NOT NULL COMMENT '来源类型: LandingForm/PlatformSearch/KeywordRecommend/BatchImport/ManualInput',
    Platform VARCHAR(30) COMMENT '平台: 抖音/小红书/视频号/公众号/朋友圈',
    AccountName VARCHAR(100) COMMENT '账号名称',
    TrackingCode VARCHAR(50) COMMENT '追踪编码',
    LandingPageId BIGINT COMMENT '关联引流页ID',
    Status TINYINT DEFAULT 1 COMMENT '1=启用 0=停用',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_leadsource_tenant (TenantId),
    INDEX idx_leadsource_tracking (TrackingCode),
    FOREIGN KEY (TenantId) REFERENCES Tenant(Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='客户来源渠道';

-- =============================================
-- 4. 客户线索表（核心实体）
-- =============================================
CREATE TABLE IF NOT EXISTS LeadCustomer (
    Id BIGINT AUTO_INCREMENT PRIMARY KEY,
    TenantId BIGINT NOT NULL COMMENT '所属租户',
    Name VARCHAR(50) COMMENT '姓名',
    Phone VARCHAR(20) COMMENT '手机号',
    WeChat VARCHAR(50) COMMENT '微信号',
    City VARCHAR(30) COMMENT '城市',
    SourcePlatform VARCHAR(30) COMMENT '来源平台',
    SourceAccount VARCHAR(100) COMMENT '来源账号',
    LeadSourceId BIGINT COMMENT '关联来源ID',
    ConsultContent TEXT COMMENT '咨询内容',
    InterestProduct VARCHAR(200) COMMENT '意向产品',
    Status VARCHAR(20) DEFAULT 'New' COMMENT '状态: New/Contacted/InProgress/Hot/Closed/Invalid',
    SourceType VARCHAR(30) DEFAULT 'ManualInput' COMMENT '来源类型',
    Remark TEXT COMMENT '备注',
    Tags JSON COMMENT '标签(JSON数组)',
    AssignedUserId BIGINT COMMENT '负责人ID',
    LastFollowUpTime DATETIME COMMENT '最后跟进时间',
    InvalidReason VARCHAR(200) COMMENT '无效原因',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_lead_tenant (TenantId),
    INDEX idx_lead_status (Status),
    INDEX idx_lead_phone (Phone),
    INDEX idx_lead_wechat (WeChat),
    INDEX idx_lead_assigned (AssignedUserId),
    INDEX idx_lead_source (LeadSourceId),
    INDEX idx_lead_created (CreatedAt),
    UNIQUE KEY uk_tenant_phone (TenantId, Phone),
    UNIQUE KEY uk_tenant_wechat (TenantId, WeChat),
    FOREIGN KEY (TenantId) REFERENCES Tenant(Id),
    FOREIGN KEY (LeadSourceId) REFERENCES LeadSource(Id),
    FOREIGN KEY (AssignedUserId) REFERENCES AppUser(Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='客户线索';

-- =============================================
-- 5. AI任务表
-- =============================================
CREATE TABLE IF NOT EXISTS AiTask (
    Id BIGINT AUTO_INCREMENT PRIMARY KEY,
    TenantId BIGINT NOT NULL COMMENT '所属租户',
    TaskType VARCHAR(30) NOT NULL COMMENT '任务类型: ContentGeneration/LeadAnalysis/MatchScoring',
    InputParams JSON COMMENT '输入参数(JSON)',
    OutputResult JSON COMMENT '输出结果(JSON)',
    Status VARCHAR(20) DEFAULT 'Queued' COMMENT '状态: Queued/Running/Success/Failed',
    PromptTemplateId BIGINT COMMENT 'Prompt模板ID',
    CostTokens INT DEFAULT 0 COMMENT 'Token消耗数',
    DurationMs INT COMMENT '耗时(毫秒)',
    ErrorMessage TEXT COMMENT '错误信息',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_aitask_tenant (TenantId),
    INDEX idx_aitask_status (Status),
    FOREIGN KEY (TenantId) REFERENCES Tenant(Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='AI任务';

-- =============================================
-- 6. AI内容表
-- =============================================
CREATE TABLE IF NOT EXISTS AiContent (
    Id BIGINT AUTO_INCREMENT PRIMARY KEY,
    TenantId BIGINT NOT NULL COMMENT '所属租户',
    AiTaskId BIGINT COMMENT '关联AI任务ID',
    ContentType VARCHAR(30) COMMENT '内容类型: ImageText/ShortVideoScript/MomentPost',
    TargetPlatform VARCHAR(30) COMMENT '目标平台: 抖音/小红书/视频号/朋友圈',
    Title VARCHAR(200) COMMENT '标题',
    Body TEXT COMMENT '正文',
    Tags JSON COMMENT '标签(JSON数组)',
    Cta VARCHAR(200) COMMENT 'CTA文案',
    Industry VARCHAR(50) COMMENT '行业',
    Product VARCHAR(100) COMMENT '产品名称',
    City VARCHAR(30) COMMENT '城市',
    LandingPageId BIGINT COMMENT '关联引流页ID',
    Status TINYINT DEFAULT 1 COMMENT '1=正常 0=已删除',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_aicontent_tenant (TenantId),
    INDEX idx_aicontent_task (AiTaskId),
    INDEX idx_aicontent_platform (TargetPlatform),
    FOREIGN KEY (TenantId) REFERENCES Tenant(Id),
    FOREIGN KEY (AiTaskId) REFERENCES AiTask(Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='AI内容';
