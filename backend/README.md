# Backend

目标技术栈：ASP.NET Core / .NET 9 + SQL Server + Redis + Hangfire。

## 项目结构规划

```text
src/GrowthAI.Api             HTTP API、认证、Controller
src/GrowthAI.Application     Use Case、DTO、Service Interface
src/GrowthAI.Domain          Entity、Enum、领域规则
src/GrowthAI.Infrastructure  EF Core、AI Provider、Cache、Jobs
tests/                       单元测试和集成测试
```

Sprint 2 开始创建真实 `.sln` 与 `.csproj`。
