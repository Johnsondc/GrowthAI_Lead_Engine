// ============================================
// 功能描述：认证与多租户模块 - 认证服务接口
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Auth.Dto;

namespace GrowthAI.Application.Auth;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<LoginResponse?> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(string token);
}
