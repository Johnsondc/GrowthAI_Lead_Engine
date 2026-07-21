-- ============================================
-- GrowthAI Lead Engine - Auth & Tenant Migration
-- Sprint 3: 认证与多租户数据库迁移
-- 生成：Qoder by 庄园
-- 生成日期：2026-07-21
-- ============================================

USE growthai_dev;

-- 租户/企业表
CREATE TABLE IF NOT EXISTS Tenant (
    Id BIGINT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL COMMENT '企业名称',
    Industry VARCHAR(50) COMMENT '行业',
    ContactPhone VARCHAR(20) COMMENT '联系电话',
    PlanType VARCHAR(20) DEFAULT 'Basic' COMMENT '套餐类型',
    Status TINYINT DEFAULT 1 COMMENT '1=启用 0=停用',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_tenant_status (Status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='租户/企业';

-- 系统用户表
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

-- 种子数据：测试企业 + 4个角色用户
INSERT INTO Tenant (Id, Name, Industry, ContactPhone, PlanType, Status) VALUES
(1, '测试母婴公司', '母婴', '13800000000', 'Pro', 1);

-- 密码统一为: Test123456 (bcrypt hash)
INSERT INTO AppUser (TenantId, Name, Phone, PasswordHash, Role, Status) VALUES
(1, '系统管理员', '13800000001', 'BCRYPT_HASH_PLACEHOLDER', 'Admin', 1),
(1, '老板', '13800000002', 'BCRYPT_HASH_PLACEHOLDER', 'Owner', 1),
(1, '运营小王', '13800000003', 'BCRYPT_HASH_PLACEHOLDER', 'Operator', 1),
(1, '销售小李', '13800000004', 'BCRYPT_HASH_PLACEHOLDER', 'Sales', 1);