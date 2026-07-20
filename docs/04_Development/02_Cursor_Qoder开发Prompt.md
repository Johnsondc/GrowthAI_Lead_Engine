# Cursor / Qoder 开发 Prompt

## 总 Prompt

你正在开发 GrowthAI Lead Engine，一个 AI 新媒体获客 SaaS。第一版只做客户池、内容中心、引流页、客户管理和数据分析。不要扩展 CRM、订单、支付、数字人、RAG 或 Agent。技术栈为 Vue3 + TypeScript + Element Plus、ASP.NET Core .NET 10、MySQL、Redis、Hangfire，并预留 DeepSeek/OpenAI/Qwen AI Provider。

## Backend Prompt

请根据 `docs/03_Architecture/01_数据库设计.md` 和 `database/migrations/001_init_mvp.sql` 创建 .NET 10 分层后端项目，包含实体、DbContext、Repository、Service、Controller 和基础单元测试。所有核心实体必须包含 TenantId、CreatedAt、UpdatedAt。

## Frontend Prompt

请根据 `docs/02_PRD/04_页面设计.md` 创建 Vue3 后台页面，优先实现 Dashboard、LeadPool、ContentCenter、LandingManager、Analytics。使用模拟 API 数据也可以，但类型和接口路径必须与 `docs/03_Architecture/02_API设计.md` 保持一致。
