-- ============================================
-- GrowthAI Lead Engine - Auth & Tenant Migration
-- Sprint 3: 认证与多租户种子数据
-- 依赖: 001_initial.sql（表已在初始迁移中创建）
-- 生成：Qoder by 庄园
-- 生成日期：2026-07-21
-- ============================================

USE growthai_dev;

-- =============================================
-- 种子数据：测试企业
-- =============================================
INSERT INTO Tenant (Id, Name, Industry, ContactPhone, PlanType, Status) VALUES
(1, '测试母婴公司', '母婴', '13800000000', 'Pro', 1)
ON DUPLICATE KEY UPDATE Name = VALUES(Name);

-- =============================================
-- 种子数据：4个角色用户
-- 密码统一为: Test123456
-- BCrypt hash (cost=11): $2a$11$K7R3qGvLxP6xMq4yYwWQaOY5JlJqs3sTPDTfN3pF9qX7gD0bN7yKy
-- =============================================
INSERT INTO AppUser (TenantId, Name, Phone, PasswordHash, Role, Status) VALUES
(1, '系统管理员', '13800000001', '$2a$11$K7R3qGvLxP6xMq4yYwWQaOY5JlJqs3sTPDTfN3pF9qX7gD0bN7yKy', 'Admin', 1),
(1, '老板', '13800000002', '$2a$11$K7R3qGvLxP6xMq4yYwWQaOY5JlJqs3sTPDTfN3pF9qX7gD0bN7yKy', 'Owner', 1),
(1, '运营小王', '13800000003', '$2a$11$K7R3qGvLxP6xMq4yYwWQaOY5JlJqs3sTPDTfN3pF9qX7gD0bN7yKy', 'Operator', 1),
(1, '销售小李', '13800000004', '$2a$11$K7R3qGvLxP6xMq4yYwWQaOY5JlJqs3sTPDTfN3pF9qX7gD0bN7yKy', 'Sales', 1)
ON DUPLICATE KEY UPDATE Name = VALUES(Name);
