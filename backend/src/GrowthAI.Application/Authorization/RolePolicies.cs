// ============================================
// 功能描述：认证与多租户模块 - 角色权限策略定义
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using System.Security.Claims;
using GrowthAI.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace GrowthAI.Application.Authorization;

/// <summary>
/// 角色权限策略常量与注册扩展
/// </summary>
public static class RolePolicies
{
    /// <summary>仅管理员可访问（企业信息/员工管理）</summary>
    public const string AdminOnly = "AdminOnly";

    /// <summary>管理员或老板可访问（数据分析/数据导出）</summary>
    public const string AdminOrOwner = "AdminOrOwner";

    /// <summary>管理员或运营可访问（客户池新建编辑/内容中心/引流页/主动获客）</summary>
    public const string AdminOrOperator = "AdminOrOperator";

    /// <summary>管理员或销售可访问（客户跟进/改状态）</summary>
    public const string AdminOrSales = "AdminOrSales";

    /// <summary>所有已认证用户（基础认证要求）</summary>
    public const string Authenticated = "Authenticated";

    /// <summary>
    /// 注册所有角色权限策略到授权服务
    /// </summary>
    public static IServiceCollection AddRolePolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AdminOnly, policy =>
                policy.RequireRole(nameof(UserRole.Admin)));

            options.AddPolicy(AdminOrOwner, policy =>
                policy.RequireRole(nameof(UserRole.Admin), nameof(UserRole.Owner)));

            options.AddPolicy(AdminOrOperator, policy =>
                policy.RequireRole(nameof(UserRole.Admin), nameof(UserRole.Operator)));

            options.AddPolicy(AdminOrSales, policy =>
                policy.RequireRole(nameof(UserRole.Admin), nameof(UserRole.Sales)));

            options.AddPolicy(Authenticated, policy =>
                policy.RequireAuthenticatedUser());
        });

        return services;
    }
}
