# CHANGELOG

## v0.3.0 (2026-07-21) - Sprint 3 认证与多租户

### 新增
- **Sprint 3 完整后端实现**：JWT认证 + 多租户隔离 + 角色权限
- `RolePolicies.cs` — 基于角色的授权策略（AdminOnly/AdminOrOwner/AdminOrOperator/AdminOrSales）
- Swagger JWT 认证配置（支持在 Swagger UI 中测试受保护接口）
- `database/migrations/001_initial.sql` — 6张核心业务表建表脚本
- `database/migrations/002_auth_tenant.sql` — 种子数据（1测试企业 + 4角色用户）

### 修复
- 4个 .csproj 文件重新生成（修复编码损坏导致的XML缺失）
- `appsettings.json` / `appsettings.Development.json` 重新生成（修复JSON格式损坏）
- `docker-compose.yml` 修复端口映射转义字符问题
- Application.csproj 添加 Infrastructure 项目引用（修复编译错误）
- Program.cs 添加 RoleClaimType 配置（修复角色校验失效）

### 技术决策
- 使用 .NET 9（LTS稳定版）替代 .NET 10 preview
- MySQL Provider: Pomelo.EntityFrameworkCore.MySql 9.0.0
- 包版本统一为 stable release

---

## v0.2.0 (历史) - 需求分析与任务规划

- `docs/BUSINESS_ARCHITECTURE.md` — 完整业务架构方案文档
- `TASK.md` — AI可执行的开发任务清单（Sprint 3-15）
- 确认三种主动获客模式全部纳入MVP

---

## v0.1.0 (历史) - 代码骨架生成

- 后端 .NET 四层项目结构
- 前端 Vue3 + TS + Element Plus 骨架
- docker-compose（MySQL 8.4 + Redis 7）
- 内存数据仓库实现
- 基础 Controller（Lead/Landing/Analytics）
- 前端 MVP 页面
