// ============================================
// 功能描述：认证与多租户模块 - 登录响应DTO
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Application.Auth.Dto;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public long ExpiresIn { get; set; }
    public UserInfo User { get; set; } = new();
}

public class UserInfo
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public long TenantId { get; set; }
}
