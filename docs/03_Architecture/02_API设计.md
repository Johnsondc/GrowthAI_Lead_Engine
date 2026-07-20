# API 设计

## Tenant

- `GET /api/tenants/current`：当前企业信息。
- `PUT /api/tenants/current`：更新企业信息。

## Lead Customer

- `GET /api/leads`：客户池列表。
- `POST /api/leads`：新增客户。
- `GET /api/leads/{id}`：客户详情。
- `PUT /api/leads/{id}`：编辑客户。
- `POST /api/leads/{id}/assign`：分配负责人。
- `POST /api/leads/{id}/status`：修改客户状态。
- `POST /api/leads/{id}/notes`：添加跟进备注。

## AI Content

- `POST /api/ai/content/generate`：生成新媒体内容。
- `GET /api/ai/content`：历史生成内容。
- `GET /api/ai/tasks/{id}`：查询任务状态。

## Landing

- `GET /api/landing/{code}`：获取引流页配置。
- `POST /api/landing/{code}/submit`：提交客户线索。

## Analytics

- `GET /api/analytics/overview`：总览统计。
- `GET /api/analytics/sources`：来源平台统计。
- `GET /api/analytics/funnel`：状态漏斗统计。
