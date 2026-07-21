// ============================================
// 鍔熻兘鎻忚堪锛氳璇佷笌澶氱鎴锋ā鍧?- 璁よ瘉鎺у埗鍣?
// 鐢熸垚锛歈oder by 搴勫洯
// 鐢熸垚鏃ユ湡锛?026-07-21
// ============================================

using GrowthAI.Application.Auth;
using GrowthAI.Application.Auth.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// 鐢ㄦ埛鐧诲綍
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        if (result == null)
            return Unauthorized(new { message = "鎵嬫満鍙锋垨瀵嗙爜閿欒" });

        return Ok(result);
    }

    /// <summary>
    /// 鍒锋柊Token
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request.RefreshToken);
        if (result == null)
            return Unauthorized(new { message = "RefreshToken鏃犳晥鎴栧凡杩囨湡" });

        return Ok(result);
    }

    /// <summary>
    /// 閫€鍑虹櫥褰?
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        await _authService.LogoutAsync(token);
        return Ok(new { message = "宸查€€鍑? });
    }

    /// <summary>
    /// 鑾峰彇褰撳墠鐢ㄦ埛淇℃伅
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            UserId = HttpContext.Items["UserId"],
            TenantId = HttpContext.Items["TenantId"],
            Name = User.Identity?.Name,
            Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }
}