// ============================================
// 功能描述：认证与多租户模块 - 认证服务实现
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Auth.Dto;
using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Repositories;

namespace GrowthAI.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IAppUserRepository _userRepository;
    private readonly JwtHelper _jwtHelper;

    // 简易RefreshToken存储（生产环境应使用Redis）
    private static readonly Dictionary<string, (long UserId, long TenantId, DateTime ExpiresAt)> _refreshTokens = new();

    public AuthService(IAppUserRepository userRepository, JwtHelper jwtHelper)
    {
        _userRepository = userRepository;
        _jwtHelper = jwtHelper;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await FindUserByPhoneAsync(request.Phone);
        if (user == null) return null;

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        var accessToken = _jwtHelper.GenerateAccessToken(user);
        var refreshToken = _jwtHelper.GenerateRefreshToken();

        _refreshTokens[refreshToken] = (user.Id, user.TenantId, DateTime.UtcNow.AddDays(7));

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = 1440 * 60,
            User = new UserInfo
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Role = user.Role.ToString(),
                TenantId = user.TenantId
            }
        };
    }

    public async Task<LoginResponse?> RefreshTokenAsync(string refreshToken)
    {
        if (!_refreshTokens.TryGetValue(refreshToken, out var tokenInfo))
            return null;

        if (tokenInfo.ExpiresAt < DateTime.UtcNow)
        {
            _refreshTokens.Remove(refreshToken);
            return null;
        }

        var user = await _userRepository.GetByIdAsync(tokenInfo.UserId);
        if (user == null || user.Status != 1) return null;

        var newAccessToken = _jwtHelper.GenerateAccessToken(user);
        var newRefreshToken = _jwtHelper.GenerateRefreshToken();

        _refreshTokens.Remove(refreshToken);
        _refreshTokens[newRefreshToken] = (user.Id, user.TenantId, DateTime.UtcNow.AddDays(7));

        return new LoginResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresIn = 1440 * 60,
            User = new UserInfo
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Role = user.Role.ToString(),
                TenantId = user.TenantId
            }
        };
    }

    public Task LogoutAsync(string token)
    {
        _refreshTokens.Remove(token);
        return Task.CompletedTask;
    }

    private async Task<AppUser?> FindUserByPhoneAsync(string phone)
    {
        // MVP简化：MVP阶段默认查询租户1，生产环境应在登录时传递TenantId
        var users = await _userRepository.GetByTenantAsync(1);
        return users.FirstOrDefault(u => u.Phone == phone && u.Status == 1);
    }
}
