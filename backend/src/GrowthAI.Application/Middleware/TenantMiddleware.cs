// ============================================
// 鍔熻兘鎻忚堪锛氳璇佷笌澶氱鎴锋ā鍧?- 绉熸埛涓婁笅鏂囦腑浠?
// 鐢熸垚锛歈oder by 搴勫洯
// 鐢熸垚鏃ユ湡锛?026-07-21
// ============================================

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GrowthAI.Application.Middleware;

/// <summary>
/// 浠嶫WT涓彁鍙朤enantId骞舵敞鍏ttpContext锛屼緵鍚庣画涓氬姟浣跨敤
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
/// HttpContext鎵╁睍鏂规硶锛屾柟渚胯幏鍙栫鎴稩D
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