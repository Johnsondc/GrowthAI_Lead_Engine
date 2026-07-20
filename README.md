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
- Backend: ASP.NET Core / .NET 9
- Database: SQL Server
- Cache/Jobs: Redis + Hangfire
- AI: DeepSeek / OpenAI / Qwen

## Repository 结构

```text
backend/       ASP.NET Core 后端骨架
database/      SQL Server 表结构与种子数据
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
- SQL Server 建表脚本
- AI 开发 Prompt 与任务清单

## 开发原则

- MVP First：第一版只做最小可收费闭环。
- Lead First：一切围绕客户线索沉淀和转化。
- Data First：所有来源、状态、转化都要可统计。
- AI Second：AI 是提效工具，不替代获客业务闭环。
