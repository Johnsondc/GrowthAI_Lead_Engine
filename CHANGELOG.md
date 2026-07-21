# GrowthAI Lead Engine - Changelog

## v0.6.0 - Sprint 6: M3 客户池
- 新增 LeadCustomer 实体（客户线索，含状态流转/Tags/分配/去重）
- 新增 FollowUpRecord 实体（跟进记录）
- 新增 LeadRepository + FollowUpRecordRepository（Infrastructure层）
- 新增 LeadService（Application层，含状态流转校验、手机/微信去重检测、分页查询）
- 新增 LeadController（10个API端点：CRUD + 状态变更 + 分配 + 跟进 + 热门 + 统计）
- 新增 004_customer_pool.sql（FollowUpRecord建表）
- 更新 AppDbContext.cs（添加LeadCustomer + FollowUpRecord映射）
- 更新 Program.cs（注册Lead相关DI服务）

## v0.5.0 - Sprint 5: M2 企业管理
- 新增 EnterpriseService + AppUserService（Application层）
- 新增 EnterpriseDto + AppUserDto（DTO定义）
- 新增 UsersController（用户CRUD，Admin权限）
- 新增 SettingsController（企业信息管理）
- 更新 Program.cs（注册Enterprise相关DI服务）

## v0.4.0 - Sprint 4: M5 AI引擎
- 新增 AiTask/AiContent/AiPromptTemplate 实体
- 新增 AiTaskType/AiTaskStatus 枚举
- 新增 IAiProvider/MockAiProvider（AI提供者抽象）
- 新增 AiService（AI任务管理：模板选择→调用→存储→频次控制）
- 新增 003_ai_engine.sql（AiPromptTemplate表 + 5条Prompt种子数据）
- 更新 AppDbContext.cs + Program.cs

## v0.3.0 - Sprint 3: 基础设施
- 四层DDD项目结构（Domain/Infrastructure/Application/Api）
- JWT认证（Access Token + Refresh Token）
- TenantMiddleware 多租户隔离
- RolePolicies 角色权限策略（Admin/Owner/Operator/Sales）
- AuthController + TenantController
- 001_initial.sql（6张核心表）+ 002_auth_tenant.sql（种子数据）
- Docker Compose（MySQL 8 + Redis 7）
- Swagger + JWT Bearer配置
