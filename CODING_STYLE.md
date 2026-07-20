# Coding Style

## Backend

- 使用 ASP.NET Core 分层结构：Api、Application、Domain、Infrastructure。
- Entity 使用 PascalCase，数据库字段使用 PascalCase。
- API 返回统一 Result 包装。
- 所有写操作记录 CreatedAt/UpdatedAt。

## Frontend

- 使用 Vue3 Composition API。
- 页面按业务模块放在 `frontend/src/views`。
- API 调用集中在 `frontend/src/api`。
- 状态管理集中在 `frontend/src/stores`。
