# GrowthAI Lead Engine — 开发任务清单（AI可执行）

- 生成人：Qoder by 庄园
- 版本：v1
- 生成日期：2026-07-21
- 当前版本：v0.2.0（代码骨架已完成）
- 目标版本：v1.0.0（MVP完整发布）

---

## 使用说明

本文档为 AI 编码工具（Qoder/Cursor）的任务执行清单。按 P0→P1→P2 顺序逐模块开发，每个模块内的任务按依赖关系排序。

**执行规则：**
- 每个任务完成后打勾 `[x]`
- 每个模块完成后执行验证任务
- 遵循 `docs/BUSINESS_ARCHITECTURE.md` 中的实体定义、API设计和协作约束
- 遵循三层架构：Controller / Service / Repository
- 所有接口必须带 TenantId 租户隔离

---

## 阶段 P0：被动获客闭环（MVP核心）

### Sprint 3：M1 认证与多租户

**目标：** 实现用户登录、JWT认证、租户隔离、角色权限

#### 3.1 数据库

- [ ] 设计 Tenant 表（如不存在）：Id, Name, Industry, ContactPhone, PlanType, Status, CreatedAt
- [ ] 设计 AppUser 表（如不存在）：Id, TenantId, Name, Phone, PasswordHash, Role, Status, CreatedAt
- [ ] 执行 MySQL 迁移脚本创建/更新表
- [ ] 插入种子数据：1个测试企业 + 4个角色用户（管理员/老板/运营/销售）

#### 3.2 后端 - Domain层

- [ ] 创建 Tenant Entity
- [ ] 创建 AppUser Entity
- [ ] 创建 Role 枚举（Admin, Owner, Operator, Sales）

#### 3.3 后端 - Infrastructure层

- [ ] 创建 ITenantRepository 接口 + TenantRepository 实现
- [ ] 创建 IAppUserRepository 接口 + AppUserRepository 实现
- [ ] 配置 EF Core DbContext，注册 Tenant 和 AppUser 映射
- [ ] 替换现有内存数据仓库为 EF Core 实现

#### 3.4 后端 - Application层

- [ ] 创建 AuthService（登录、刷新Token、登出）
- [ ] 创建 IAuthService 接口
- [ ] 创建 JwtHelper 工具类（签发Token、验证Token、提取TenantId和UserId）
- [ ] 创建 TenantMiddleware 中间件（从JWT提取TenantId注入HttpContext）
- [ ] 创建角色权限 Policy（基于角色的Authorize配置）

#### 3.5 后端 - Api层

- [ ] 创建 AuthController：POST /api/auth/login, POST /api/auth/refresh, POST /api/auth/logout
- [ ] 创建 LoginRequest/RefreshTokenRequest DTO
- [ ] 配置 JWT Bearer 认证中间件
- [ ] 注册 TenantMiddleware 到 Pipeline
- [ ] 配置 CORS 策略

#### 3.6 验证

- [ ] 登录接口可正确签发 JWT
- [ ] Token 可正确解析出 TenantId 和 UserId
- [ ] 未登录请求返回 401
- [ ] 角色权限校验生效

---

### Sprint 4：M5 AI引擎

**目标：** 搭建统一的AI调用基础设施

#### 4.1 数据库

- [ ] 设计 AiPromptTemplate 表：Id, Name, Industry, Platform, ContentType, PromptText, Variables(JSON), Status, CreatedAt
- [ ] 更新 AiTask 表：补充 TenantId, PromptTemplateId, CostTokens 字段
- [ ] 更新 AiContent 表：补充 TargetPlatform, ContentType, LandingPageId 字段
- [ ] 执行迁移脚本

#### 4.2 后端 - Domain层

- [ ] 创建 AiPromptTemplate Entity
- [ ] 更新 AiTask Entity（补充新字段）
- [ ] 更新 AiContent Entity（补充新字段）
- [ ] 创建 AiTaskType 枚举（ContentGeneration, LeadAnalysis, MatchScoring）
- [ ] 创建 AiTaskStatus 枚举（Queued, Running, Success, Failed）

#### 4.3 后端 - Infrastructure层

- [ ] 创建 IAiPromptTemplateRepository + AiPromptTemplateRepository
- [ ] 更新 AiTaskRepository（适配新字段）
- [ ] 更新 AiContentRepository（适配新字段）
- [ ] 插入 Prompt 模板种子数据（母婴行业-抖音/小红书/视频号/朋友圈各一套）

#### 4.4 后端 - Application层

- [ ] 定义 IAiProvider 接口（通用AI调用：SendMessage，返回文本结果）
- [ ] 创建 MockAiProvider 实现（V1阶段使用，返回模拟数据）
- [ ] 创建 AiService：提交任务、查询结果、结果解析存储
- [ ] 创建 IAiService 接口
- [ ] 实现按行业+平台+内容类型选择Prompt模板的逻辑
- [ ] 实现 AI 调用频次控制（简单计数器，V1不依赖Redis）

#### 4.5 验证

- [ ] MockAiProvider 可正确返回模拟内容
- [ ] AiService 提交任务后可追踪状态
- [ ] Prompt 模板可按行业+平台正确选取

---

### Sprint 5：M2 企业管理

**目标：** 企业信息维护、员工管理、角色配置

#### 5.1 后端 - DTO

- [ ] 创建 EnterpriseInfoDto：Id, Name, Industry, ContactPhone, PlanType, Status
- [ ] 创建 UpdateEnterpriseRequest：Name, Industry, ContactPhone
- [ ] 创建 AppUserDto：Id, Name, Phone, Role, Status, CreatedAt
- [ ] 创建 CreateAppUserRequest：Name, Phone, Password, Role
- [ ] 创建 UpdateAppUserRequest：Name, Phone, Role, Status

#### 5.2 后端 - Application层

- [ ] 创建 EnterpriseService（获取/更新企业信息）
- [ ] 创建 AppUserService（员工CRUD）
- [ ] 对应接口定义

#### 5.3 后端 - Api层

- [ ] 创建 SettingsController：GET/PUT /api/settings/enterprise
- [ ] 创建 UsersController：GET/POST/PUT/DELETE /api/settings/users
- [ ] 配置权限：仅管理员角色可访问

#### 5.4 前端

- [ ] 创建 Settings 页面：企业信息表单（名称/行业/联系电话）
- [ ] 创建用户管理页面：员工列表（表格）+ 新增/编辑弹窗
- [ ] 对接后端 API

#### 5.5 验证

- [ ] 管理员可修改企业信息
- [ ] 管理员可增删改查员工
- [ ] 非管理员角色无法访问企业管理

---

### Sprint 6：M3 客户池

**目标：** 客户CRUD、状态流转、负责人分配、跟进记录

#### 6.1 数据库

- [ ] 更新 LeadCustomer 表：补充 Tags(JSON), AssignedUserId, LastFollowUpTime, InvalidReason, SourceType 字段
- [ ] 设计 FollowUpRecord 表：Id, TenantId, LeadCustomerId, FollowerId, FollowType, Content, NextFollowUpTime, CreatedAt
- [ ] 执行迁移脚本

#### 6.2 后端 - Domain层

- [ ] 更新 LeadCustomer Entity（补充新字段）
- [ ] 创建 FollowUpRecord Entity
- [ ] 创建 LeadStatus 枚举（New, Contacted, InProgress, Hot, Closed, Invalid）
- [ ] 创建 LeadSourceType 枚举（LandingForm, PlatformSearch, KeywordRecommend, BatchImport, ManualInput）
- [ ] 实现状态流转校验逻辑（Domain层）

#### 6.3 后端 - DTO

- [ ] 创建 LeadCustomerDto（聚合模型，包含跟进记录列表）
- [ ] 创建 FollowUpRecordDto（子Item DTO）
- [ ] 创建 CreateLeadCustomerRequest
- [ ] 创建 UpdateLeadCustomerRequest
- [ ] 创建 UpdateLeadStatusRequest（Status, InvalidReason）
- [ ] 创建 AssignLeadRequest（AssignedUserId）
- [ ] 创建 CreateFollowUpRequest（FollowType, Content, NextFollowUpTime）
- [ ] 创建 LeadQueryRequest（Status, SourcePlatform, AssignedUserId, City, Keyword, DateRange, Page, PageSize）

#### 6.4 后端 - Infrastructure层

- [ ] 更新 ILeadRepository：新增分页查询、状态更新、负责人分配方法
- [ ] 创建 IFollowUpRecordRepository + FollowUpRecordRepository
- [ ] EF Core DbContext 注册新表

#### 6.5 后端 - Application层

- [ ] 创建 LeadService：列表查询（分页/筛选）、详情、新建、编辑、删除、状态更新、负责人分配
- [ ] 实现留资校验：手机号或微信至少填一个，同一租户下手机号/微信唯一
- [ ] 实现状态流转规则校验
- [ ] 创建 FollowUpService（或在LeadService内）：新增跟进记录、查询跟进历史
- [ ] 对应接口定义

#### 6.6 后端 - Api层

- [ ] 更新 LeadController：
  - GET /api/leads（分页+筛选）
  - POST /api/leads（新建）
  - GET /api/leads/{id}（详情）
  - PUT /api/leads/{id}（编辑）
  - DELETE /api/leads/{id}（删除）
  - PUT /api/leads/{id}/status（状态更新）
  - PUT /api/leads/{id}/assign（分配负责人）
  - POST /api/leads/{id}/follow-ups（新增跟进）
  - GET /api/leads/{id}/follow-ups（跟进历史）

#### 6.7 前端

- [ ] 重构客户池页面：完整表格列（姓名/手机/微信/城市/来源/意向产品/状态/负责人/创建时间）
- [ ] 新增筛选区：状态、来源平台、负责人、城市、关键词搜索、时间范围
- [ ] 新增客户弹窗：表单含所有必填/选填字段 + 校验
- [ ] 编辑客户弹窗
- [ ] 客户详情页：基本信息 + 跟进记录时间线 + 状态流转操作
- [ ] 状态变更下拉框（校验只允许正向流转）
- [ ] 分配负责人下拉框
- [ ] 新增跟进记录弹窗
- [ ] 对接后端所有 API

#### 6.8 验证

- [ ] 客户列表分页+筛选正常
- [ ] 新建客户：手机号/微信至少填一个的校验生效
- [ ] 手机号/微信重复检测
- [ ] 状态只能正向流转，终态不可变更
- [ ] 负责人可分配
- [ ] 跟进记录可添加和查看

---

### Sprint 7：M7 来源管理

**目标：** 客户来源渠道配置与追踪

#### 7.1 数据库

- [ ] 更新 LeadSource 表：补充 TrackingCode, LandingPageId 字段
- [ ] 执行迁移脚本

#### 7.2 后端

- [ ] 创建 LeadSourceDto、CreateLeadSourceRequest、UpdateLeadSourceRequest
- [ ] 创建 LeadSourceService（CRUD + 生成TrackingCode）
- [ ] 创建 SourceController：GET/POST /api/sources, GET/PUT/DELETE /api/sources/{id}
- [ ] 更新客户新建时关联 LeadSource

#### 7.3 前端

- [ ] 在设置模块新增"来源渠道"管理页面：渠道列表 + 新增弹窗
- [ ] 渠道列表显示：平台名称、账号名称、TrackingCode、关联客户数

#### 7.4 验证

- [ ] 渠道CRUD正常
- [ ] 新建客户时可选择关联来源
- [ ] 来源效果统计（关联客户数）正确

---

### Sprint 8：M4 内容中心

**目标：** AI驱动的多平台内容生成与管理

#### 8.1 后端 - DTO

- [ ] 创建 AiContentDto（聚合模型）
- [ ] 创建 GenerateContentRequest：Industry, Product, City, TargetCustomer, SellingPoint, Platform, ContentType
- [ ] 创建 UpdateContentRequest：Title, Body, Tags, Cta

#### 8.2 后端 - Application层

- [ ] 创建 ContentService：AI生成内容、内容CRUD、按平台适配输出格式
- [ ] 实现平台适配规则（抖音/小红书/视频号/朋友圈的长度和格式限制）
- [ ] 对接 AiService 进行内容生成

#### 8.3 后端 - Api层

- [ ] 创建 ContentController：
  - POST /api/contents/generate（AI生成）
  - GET/POST /api/contents（列表/新建）
  - GET/PUT/DELETE /api/contents/{id}（详情/编辑/删除）

#### 8.4 前端

- [ ] 重构内容中心页面：
  - 生成区：输入参数表单（行业/产品/城市/目标客户/卖点）+ 平台选择 + 内容类型选择
  - 结果展示：标题/正文/标签/CTA，可编辑
  - 内容列表：已生成内容的管理表格
- [ ] 对接后端 API

#### 8.5 验证

- [ ] AI 生成接口可返回内容（Mock模式下返回模板数据）
- [ ] 不同平台返回不同格式和长度限制
- [ ] 内容可编辑保存
- [ ] 内容列表CRUD正常

---

### Sprint 9：M6 引流页

**目标：** H5留资页面创建、管理、客户自动入库

#### 9.1 数据库

- [ ] 设计 LandingPage 表：Id, TenantId, Title, Description, FormConfig(JSON), RelatedContentId, BackgroundImage, Status(Enabled/Disabled), VisitCount, SubmitCount, ShortCode, CreatedAt, UpdatedAt
- [ ] 执行迁移脚本

#### 9.2 后端 - Domain层

- [ ] 创建 LandingPage Entity
- [ ] 创建 LandingPageStatus 枚举（Enabled, Disabled）

#### 9.3 后端 - DTO

- [ ] 创建 LandingPageDto
- [ ] 创建 CreateLandingPageRequest
- [ ] 创建 UpdateLandingPageRequest
- [ ] 创建 LandingPageSubmitRequest：Name, Phone, WeChat, City, Description（C端表单提交）

#### 9.4 后端 - Infrastructure层

- [ ] 创建 ILandingPageRepository + LandingPageRepository
- [ ] EF Core DbContext 注册 LandingPage

#### 9.5 后端 - Application层

- [ ] 创建 LandingPageService：CRUD、发布/停用、访问计数、提交处理
- [ ] 提交处理逻辑：校验 → 去重 → 创建LeadCustomer（SourceType=LandingForm） → 更新提交计数

#### 9.6 后端 - Api层

- [ ] 创建 LandingPageController（B端管理）：
  - GET/POST /api/landing-pages
  - GET/PUT /api/landing-pages/{id}
  - POST /api/landing-pages/{id}/publish
  - POST /api/landing-pages/{id}/disable
- [ ] 创建 LandingSubmitController（C端提交，无需认证）：
  - GET /api/l/{shortcode}（获取引流页配置）
  - POST /api/l/{shortcode}/submit（提交留资）

#### 9.7 前端

- [ ] 引流页管理页面：列表 + 创建/编辑弹窗
- [ ] 引流页预览功能（模拟H5表单页面）
- [ ] 二维码生成（引流页URL → 二维码图片）
- [ ] C端留资页面（独立路由，简洁H5表单）
- [ ] 提交成功后显示"感谢您的咨询，我们将尽快与您联系"

#### 9.8 验证

- [ ] 引流页CRUD正常
- [ ] 发布后生成唯一短码URL
- [ ] C端表单提交后客户自动进入客户池
- [ ] 重复提交不重复入库（更新咨询内容）
- [ ] 提交计数正确递增

---

## 阶段 P1：主动获客 + 数据分析

### Sprint 10：M8 数据分析

**目标：** 来源统计、漏斗分析、趋势图表

#### 10.1 后端 - DTO

- [ ] 创建 OverviewDto：TodayNew, TotalLeads, EffectiveLeads, SourceDistribution, StatusFunnel
- [ ] 创建 SourceStatDto：Platform, AccountName, Count, Percentage
- [ ] 创建 FunnelStatDto：Status, Count, Percentage
- [ ] 创建 TrendStatDto：Date, NewCount, EffectiveCount
- [ ] 创建 AnalyticsQueryRequest：DateRange, Platform

#### 10.2 后端 - Application层

- [ ] 创建 AnalyticsService：总览数据、来源分布、状态漏斗、新增趋势
- [ ] 所有查询基于 LeadCustomer 实时数据聚合

#### 10.3 后端 - Api层

- [ ] 创建 AnalyticsController：
  - GET /api/analytics/overview
  - GET /api/analytics/sources
  - GET /api/analytics/funnel
  - GET /api/analytics/trends

#### 10.4 前端

- [ ] 数据分析页面：
  - 顶部数据卡片：今日新增、总客户数、有效客户数
  - 来源分布饼图/表格
  - 客户状态漏斗图
  - 新增趋势折线图
  - 时间范围筛选器
- [ ] 对接后端 API

#### 10.5 验证

- [ ] 总览数据与实际客户数一致
- [ ] 来源分布按平台正确聚合
- [ ] 漏斗按状态正确统计
- [ ] 趋势图按日期正确展示

---

### Sprint 11：M11 模式C - 批量导入

**目标：** Excel/CSV上传、智能映射、去重导入

#### 11.1 数据库

- [ ] 设计 LeadImportLog 表：Id, TenantId, FileName, TotalRows, SuccessCount, UpdateCount, SkipCount, ErrorCount, Status, CreatedAt, ErrorDetails(JSON)

#### 11.2 后端 - Domain层

- [ ] 创建 LeadImportLog Entity
- [ ] 创建 ImportStatus 枚举（Uploading, Parsing, Preview, Executing, Completed, Failed）

#### 11.3 后端 - DTO

- [ ] 创建 ImportPreviewDto：TotalRows, MappedFields, PreviewRows(前5行), DuplicateCount, ErrorRows
- [ ] 创建 ImportResultDto：ImportLogId, SuccessCount, UpdateCount, SkipCount, ErrorCount, ErrorDetails
- [ ] 创建 ImportExecuteRequest：FieldMapping(JSON), ImportMode(InsertOnly/UpdateExisting/Both)

#### 11.4 后端 - Infrastructure层

- [ ] 创建 ILeadImportLogRepository + LeadImportLogRepository

#### 11.5 后端 - Application层

- [ ] 创建 LeadImportService：
  - 上传文件（临时存储）
  - 解析Excel/CSV（EPPlus）
  - 智能字段映射（列名模糊匹配系统字段）
  - 数据预校验（格式+必填项）
  - 去重检测（手机号+微信）
  - 执行批量导入（新增+更新）
  - 生成导入报告

#### 11.6 后端 - Api层

- [ ] 创建 LeadImportController：
  - POST /api/acquisition/import/upload（上传文件，返回预览）
  - POST /api/acquisition/import/execute（确认导入）
  - GET /api/acquisition/import/reports/{id}（查看报告）
  - GET /api/acquisition/import/template（下载模板）

#### 11.7 前端

- [ ] 批量导入功能页面：
  - 下载模板按钮
  - 文件上传组件（拖拽上传 Excel/CSV）
  - 字段映射预览表格（可手动调整映射）
  - 数据预览（前5行）+ 统计（新增/更新/跳过/错误）
  - 确认导入按钮
  - 导入结果展示（成功/失败明细）
- [ ] 对接后端 API

#### 11.8 验证

- [ ] Excel 文件解析正确
- [ ] 字段映射智能识别准确
- [ ] 重复数据正确跳过/更新
- [ ] 导入后客户池数据正确
- [ ] 导入报告详细准确

---

### Sprint 12：M9 模式A - 平台采集

**目标：** 手动搜索平台公开线索，筛选入库

#### 12.1 数据库

- [ ] 设计 AcquisitionTask 表：Id, TenantId, TaskType(A/B/C), SearchConfig(JSON), Platform, Keywords, City, TimeRange, Status, CreatedAt
- [ ] 设计 AcquisitionResult 表：Id, TenantId, AcquisitionTaskId, Nickname, Platform, ContentSummary, PublishTime, AiScore, ReviewStatus(Pending/Approved/Rejected), LeadCustomerId(可空), ReviewedBy, ReviewedAt, CreatedAt

#### 12.2 后端 - Domain层

- [ ] 创建 AcquisitionTask Entity
- [ ] 创建 AcquisitionResult Entity
- [ ] 创建 AcquisitionTaskType 枚举（PlatformCollect, KeywordMonitor, BatchImport）
- [ ] 创建 ReviewStatus 枚举（Pending, Approved, Rejected）

#### 12.3 后端 - DTO

- [ ] 创建 AcquisitionTaskDto
- [ ] 创建 CreateCollectTaskRequest：Platform, Keywords, City, TimeRange
- [ ] 创建 AcquisitionResultDto
- [ ] 创建 ImportResultToLeadRequest：LeadCustomerId(可选), SourceNote

#### 12.4 后端 - Infrastructure层

- [ ] 创建 IAcquisitionTaskRepository + AcquisitionTaskRepository
- [ ] 创建 IAcquisitionResultRepository + AcquisitionResultRepository

#### 12.5 后端 - Application层

- [ ] 创建 PlatformCollectService：
  - 创建采集任务
  - 执行采集（V1调用Mock采集服务，返回模拟公开数据）
  - 结果存储
- [ ] 定义 IPlatformCollector 接口（插件化，后续替换真实采集服务）
- [ ] 创建 MockPlatformCollector 实现
- [ ] 实现审核入库逻辑：Approved → 创建LeadCustomer（SourceType=PlatformSearch）

#### 12.6 后端 - Api层

- [ ] 创建 AcquisitionCollectController：
  - POST /api/acquisition/collect（创建并执行采集任务）
  - GET /api/acquisition/collect/tasks（任务列表）
  - GET /api/acquisition/collect/tasks/{id}/results（任务结果列表）
  - POST /api/acquisition/results/{id}/import（审核通过，加入客户池）
  - POST /api/acquisition/results/{id}/reject（拒绝）
  - POST /api/acquisition/results/batch-import（批量审核通过）

#### 12.7 前端

- [ ] 主动获客-平台采集页面：
  - 搜索表单：平台选择、关键词、城市、时间范围
  - 搜索结果列表：昵称、平台、内容摘要、发布时间
  - 勾选框 + 批量"加入客户池"按钮
  - 审核状态标记（待审核/已入库/已拒绝）
- [ ] 对接后端 API

#### 12.8 验证

- [ ] 采集任务创建正常
- [ ] Mock采集返回模拟数据
- [ ] 审核通过后客户正确入库（SourceType=PlatformSearch）
- [ ] 拒绝后不影响客户池
- [ ] 批量审核正常

---

### Sprint 13：M10 模式B - 关键词监控

**目标：** 配置关键词 → 定时搜索 → AI评分 → 推送审核

#### 13.1 数据库

- [ ] 设计 AcquisitionKeyword 表：Id, TenantId, Keyword, Industry, Platform, City, IsEnabled, CreatedAt
- [ ] 执行迁移脚本

#### 13.2 后端 - Domain层

- [ ] 创建 AcquisitionKeyword Entity

#### 13.3 后端 - DTO

- [ ] 创建 AcquisitionKeywordDto
- [ ] 创建 CreateKeywordRequest：Keyword, Industry, Platform, City
- [ ] 创建 RecommendationDto（复用AcquisitionResultDto，补充AI评分展示）

#### 13.4 后端 - Infrastructure层

- [ ] 创建 IAcquisitionKeywordRepository + AcquisitionKeywordRepository

#### 13.5 后端 - Application层

- [ ] 创建 KeywordMonitorService：
  - 关键词CRUD
  - 定时任务执行逻辑：读取所有启用的关键词 → 调用采集服务 → AI评分 → 过滤低分 → 存储结果
  - 审核列表查询
  - 审核通过/拒绝
- [ ] 创建 KeywordMonitorJob（BackgroundService / Quartz）：
  - 每日定时触发（默认每天 08:00）
  - 遍历所有租户的启用关键词
  - 调用 IPlatformCollector 执行搜索
  - 调用 AiService 进行匹配度评分
  - 结果过滤（低于阈值丢弃）
  - 存储结果 + 推送通知

#### 13.6 后端 - Api层

- [ ] 创建 AcquisitionKeywordController：
  - GET/POST /api/acquisition/keywords
  - PUT/DELETE /api/acquisition/keywords/{id}
  - POST /api/acquisition/keywords/{id}/toggle（启用/停用）
- [ ] 创建 AcquisitionRecommendationController：
  - GET /api/acquisition/recommendations（推荐列表，分页+筛选）
  - POST /api/acquisition/recommendations/{id}/approve
  - POST /api/acquisition/recommendations/{id}/reject

#### 13.7 前端

- [ ] 关键词配置页面：
  - 关键词列表（关键词/行业/平台/城市/启用状态）
  - 新增/编辑弹窗
  - 启用/停用开关
- [ ] 线索推荐页面：
  - 推荐列表：昵称、平台、内容摘要、AI评分、审核状态
  - 筛选：平台、评分范围、审核状态
  - 逐条审核通过/拒绝按钮
  - 批量审核通过
- [ ] 对接后端 API

#### 13.8 验证

- [ ] 关键词CRUD正常
- [ ] 定时任务可手动触发
- [ ] AI评分结果正确
- [ ] 审核通过后客户入库（SourceType=KeywordRecommend）
- [ ] 低分结果被正确过滤

---

## 阶段 P2：优化提升

### Sprint 14：M12 通知中心

**目标：** 站内消息推送与管理

#### 14.1 数据库

- [ ] 设计 Notification 表：Id, TenantId, UserId, Type(LeadImported/KeywordRecommend/TaskCompleted/AcquisitionReady), Title, Content, IsRead, CreatedAt

#### 14.2 后端

- [ ] 创建 Notification Entity + DTO
- [ ] 创建 INotificationRepository + NotificationRepository
- [ ] 创建 NotificationService：发送通知、查询列表、标记已读、全部已读
- [ ] 创建 NotificationController：
  - GET /api/notifications（列表，分页）
  - PUT /api/notifications/{id}/read（标记已读）
  - PUT /api/notifications/read-all（全部已读）
  - GET /api/notifications/unread-count（未读数）
- [ ] 在关键业务节点触发通知：批量导入完成、关键词推荐就绪、采集任务完成

#### 14.3 前端

- [ ] 顶部导航栏通知铃铛图标 + 未读数角标
- [ ] 通知下拉列表（最近10条）
- [ ] 通知全量页面：消息列表 + 已读/未读筛选
- [ ] 点击通知跳转到对应页面

#### 14.4 验证

- [ ] 关键业务操作后通知正确推送
- [ ] 未读数正确显示
- [ ] 标记已读正常
- [ ] 点击通知跳转正确

---

### Sprint 15：M13 数据导入导出

**目标：** 客户数据导出Excel

#### 15.1 后端

- [ ] 创建 DataExportService：客户列表导出（按当前筛选条件）
- [ ] 创建 ExportController：
  - GET /api/export/leads（下载Excel文件）
- [ ] 使用 EPPlus 生成 Excel 文件

#### 15.2 前端

- [ ] 客户池页面新增"导出"按钮
- [ ] 点击导出按当前筛选条件下载 Excel

#### 15.3 验证

- [ ] 导出数据与当前筛选结果一致
- [ ] Excel 格式正确，字段完整
- [ ] 大数据量导出不超时

---

## 总体验证（v1.0 Release）

- [ ] 完整走通被动获客流程：AI生成内容 → 创建引流页 → 客户留资 → 自动入库 → 销售跟进 → 状态流转
- [ ] 完整走通主动获客-批量导入：上传Excel → 预览映射 → 确认导入 → 客户入库
- [ ] 完整走通主动获客-平台采集：搜索条件 → 采集结果 → 审核入库
- [ ] 完整走通主动获客-关键词监控：配置关键词 → 定时搜索 → AI评分 → 推送 → 审核入库
- [ ] 数据分析：来源统计、漏斗、趋势与实际数据一致
- [ ] 多租户隔离：不同企业数据完全隔离
- [ ] 权限控制：各角色只能访问授权功能
- [ ] 所有 API 响应规范一致（统一返回格式）
- [ ] 前端页面无明显UI问题

---

## 附：技术约束备忘

| 约束项 | 说明 |
|--------|------|
| 后端框架 | .NET 10 (net10.0) |
| 数据库 | MySQL 8 + EF Core |
| 缓存 | Redis 7 |
| 前端框架 | Vue3 + TypeScript + Element Plus + Pinia |
| 部署 | Docker Compose |
| 代码规范 | 三层架构（Controller/Service/Repository），每类单独文件 |
| DTO规范 | 每模块唯一主DTO + QueryDto，禁止衍生模型 |
| 租户隔离 | 所有业务接口必须带 TenantId |
| 文件头注释 | 每个文件包含功能描述、生成人、生成日期 |
