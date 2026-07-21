# CHANGELOG

## v0.3.0 (2026-07-21) - 需求分析与任务规划

### 新增
- `docs/BUSINESS_ARCHITECTURE.md` — 完整业务架构方案文档
  - 业务概述（双向获客闭环：被动+主动）
  - 13个业务实体识别及关系定义
  - 13个功能模块划分（含优先级P0/P1/P2）
  - 模块依赖关系图（Mermaid）
  - 各模块详细功能点、校验规则、状态流转
  - API端点设计（RESTful风格）
  - 数据库表规划（6张已有 + 9张新增）
  - 风险与应对建议
  - 技术决策建议

- `TASK.md` — AI可执行的开发任务清单
  - Sprint 3-15 逐模块任务拆解
  - 每个任务为可打勾的 checklist 格式
  - 每个Sprint包含数据库、后端（Domain/DTO/Infrastructure/Application/Api）、前端、验证四部分
  - 覆盖完整MVP：认证→AI引擎→企业管理→客户池→来源→内容→引流页→分析→批量导入→平台采集→关键词监控→通知→导出
  - 总体验证清单（v1.0 Release）
  - 技术约束备忘

### 关键决策
- 确认三种主动获客模式全部纳入MVP（不再推迟到V2）
- 开发顺序：P0(被动闭环) → P1(主动+分析) → P2(通知+导出)
- 预估总工时：38-51天

---

## v0.2.0 (历史) - 代码骨架生成

- 后端 .NET 10 四层项目结构
- 前端 Vue3 + TS + Element Plus 骨架
- docker-compose（MySQL 8.4 + Redis 7）
- MySQL 迁移脚本（6张核心表）
- 内存数据仓库实现
- 基础 Controller（Lead/Landing/Analytics）
- 前端 MVP 页面（Dashboard/LeadPool/ContentCenter/Landing/Analytics/Settings）

---

## v0.1.0 (历史) - 文档初始化

- README、项目定位、MVP目标
- 视频拆解、竞品功能矩阵
- MVP PRD、用户流程、系统架构
- 商业模式、母婴垂直版、字段字典
- 验收标准、角色权限、状态流转
- Landing模板、服务模块设计、安全合规、部署方案
