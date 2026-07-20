# GrowthAI Lead Engine

> AI New Media Lead Generation Platform / 企业 AI 新媒体获客引擎

- Version: v0.1.0
- Status: Sprint 1 Repository Initialization
- Core goal: 帮助企业持续从抖音、小红书、视频号、公众号、朋友圈、二维码、H5 表单等渠道获取精准客户线索。

## 产品定位

GrowthAI Lead Engine 不是单纯的 AI 写作工具、CRM、ERP 或数字人系统，而是一个围绕“新媒体内容 → 客户咨询 → 引流承接 → 客户池沉淀 → 销售跟进 → 数据分析”的 AI 线索增长系统。

## MVP 闭环

```text
新媒体内容
  ↓
吸引客户咨询
  ↓
AI/工具承接
  ↓
Landing Page / 二维码 / 表单留资
  ↓
客户进入 Lead Pool
  ↓
销售跟进
  ↓
数据统计与优化
```

## V1.0 只做五个模块

1. Lead / 客户池
2. Content / 内容中心
3. Landing / 引流页
4. Customer / 客户管理
5. Analytics / 数据分析

其他高级能力（AI 私信、自动发布、数字人、RAG、Agent、复杂工作流）全部延后。

## 技术栈规划

- Frontend: Vue 3 + TypeScript + Element Plus
- Backend: ASP.NET Core / .NET 10
- Database: MySQL
- Cache/Jobs: Redis + Hangfire
- AI: DeepSeek / OpenAI / Qwen

## Repository 结构

```text
backend/       ASP.NET Core 后端骨架
database/      MySQL 表结构与种子数据
frontend/      Vue3 前端骨架
docs/          产品、竞品、PRD、架构、开发文档
prompts/       Cursor/Qoder/AI 编码 Prompt
scripts/       辅助脚本
TASK.md        Sprint 任务清单
CHANGELOG.md   版本变更记录
```

## 文档入口

完整文档索引见 `docs/README.md`。

## Sprint 1 交付内容

- 项目定位与 MVP 目标
- 视频逆向要点与竞品功能矩阵
- MVP PRD、用户流程、页面清单
- 系统架构、数据库设计、API 设计
- 前后端目录骨架
- MySQL 建表脚本
- AI 开发 Prompt 与任务清单

## 开发原则

- MVP First：第一版只做最小可收费闭环。
- Lead First：一切围绕客户线索沉淀和转化。
- Data First：所有来源、状态、转化都要可统计。
- AI Second：AI 是提效工具，不替代获客业务闭环。

## 本地启动（目标环境）

> 当前项目已切换到 .NET 10 + MySQL 8。当前容器缺少 dotnet CLI，且 npm registry 对部分前端依赖返回 403；在正常开发机上按以下步骤启动。

```bash
docker compose up -d mysql redis
cd backend
dotnet restore
dotnet run --project src/GrowthAI.Api/GrowthAI.Api.csproj
```

```bash
cd frontend
npm install
npm run dev
```

后端健康检查：`GET http://localhost:5000/health`。

## 当前功能对齐

- 客户池：后端支持列表、创建、状态修改；前端有客户池列表页。
- 内容中心：后端提供 AI 内容模板生成接口；前端有内容生成表单。
- 引流页：后端提供配置读取和留资提交接口；前端有模拟提交表单。
- 数据分析：后端支持总览、来源分布、状态漏斗；前端有基础展示页。
- 企业管理：已有实体和设置页占位，员工/角色 CRUD 下一步继续生成。
