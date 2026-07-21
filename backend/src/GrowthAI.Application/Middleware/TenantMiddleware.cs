// ============================================
// 功能描述：认证与多租户模块 - 租户上下文中件
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GrowthAI.Application.Middleware;

/// <summary>
/// 从JWT中提取TenantId并注入HttpContext，供后续业务使用
/// </summary>
public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantIdClaim = context.User.FindFirst("TenantId");
            if (tenantIdClaim != null && long.TryParse(tenantIdClaim.Value, out var tenantId))
            {
                context.Items["TenantId"] = tenantId;
            }

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out var userId))
            {
                context.Items["UserId"] = userId;
            }
        }

        await _next(context);
    }
}

/// <summary>
/// HttpContext扩展方法，方便获取租户ID
/// </summary>
public static class HttpContextExtensions
{
    public static long GetTenantId(this HttpContext context)
    {
        return context.Items.TryGetValue("TenantId", out var tenantId) ? (long)tenantId! : 0;
    }

    public static long GetUserId(this HttpContext context)
    {
        return context.Items.TryGetValue("UserId", out var userId) ? (long)userId! : 0;
    }
}
