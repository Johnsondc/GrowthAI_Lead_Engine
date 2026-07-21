-- ============================================
-- GrowthAI Lead Engine - Landing Page Migration
-- Sprint 9: 引流页模块 - LandingPage表
-- 生成：Qoder by 庄园
-- 生成日期：2026-07-21
-- ============================================

USE growthai_dev;

-- =============================================
-- 8. 引流页/落地页表
-- =============================================
CREATE TABLE IF NOT EXISTS LandingPage (
    Id BIGINT AUTO_INCREMENT PRIMARY KEY,
    TenantId BIGINT NOT NULL COMMENT '所属租户',
    Title VARCHAR(200) NOT NULL COMMENT '页面标题',
    Description TEXT COMMENT '页面描述',
    CoverImage VARCHAR(500) COMMENT '封面图URL',
    PageCode VARCHAR(50) NOT NULL COMMENT '页面唯一编码(用于URL访问)',
    FormConfig JSON COMMENT '表单配置(JSON)',
    CustomCss TEXT COMMENT '自定义CSS',
    ThankYouMessage VARCHAR(500) COMMENT '提交成功提示语',
    RedirectUrl VARCHAR(500) COMMENT '提交后跳转URL',
    ViewCount INT DEFAULT 0 COMMENT '浏览量',
    SubmitCount INT DEFAULT 0 COMMENT '提交量',
    Status TINYINT DEFAULT 1 COMMENT '1=上线 0=下线',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_landingpage_tenant (TenantId),
    UNIQUE KEY uk_pagecode (PageCode),
    FOREIGN KEY (TenantId) REFERENCES Tenant(Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='引流页/落地页';
