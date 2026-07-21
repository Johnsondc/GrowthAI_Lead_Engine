-- ============================================
-- GrowthAI Lead Engine - Customer Pool Migration
-- Sprint 6: 客户池模块 - FollowUpRecord表
-- 生成：Qoder by 庄园
-- 生成日期：2026-07-21
-- ============================================

USE growthai_dev;

-- =============================================
-- 7. 跟进记录表
-- =============================================
CREATE TABLE IF NOT EXISTS FollowUpRecord (
    Id BIGINT AUTO_INCREMENT PRIMARY KEY,
    TenantId BIGINT NOT NULL COMMENT '所属租户',
    LeadCustomerId BIGINT NOT NULL COMMENT '关联客户ID',
    FollowerId BIGINT NOT NULL COMMENT '跟进人ID',
    FollowType VARCHAR(20) DEFAULT 'Phone' COMMENT '跟进方式: Phone/WeChat/Visit/Video/Other',
    Content TEXT NOT NULL COMMENT '跟进内容',
    NextFollowUpTime DATETIME COMMENT '下次跟进时间',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_followup_tenant (TenantId),
    INDEX idx_followup_lead (LeadCustomerId),
    INDEX idx_followup_follower (FollowerId),
    INDEX idx_followup_next (NextFollowUpTime),
    FOREIGN KEY (TenantId) REFERENCES Tenant(Id),
    FOREIGN KEY (LeadCustomerId) REFERENCES LeadCustomer(Id),
    FOREIGN KEY (FollowerId) REFERENCES AppUser(Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='跟进记录';
