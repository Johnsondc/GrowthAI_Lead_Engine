// ============================================
// 鍔熻兘鎻忚堪锛氳璇佷笌澶氱鎴锋ā鍧?- 璁よ瘉鏈嶅姟瀹炵幇
// 鐢熸垚锛歈oder by 搴勫洯
// 鐢熸垚鏃ユ湡锛?026-07-21
// ============================================

using GrowthAI.Application.Auth.Dto;
using GrowthAI.Domain.Entities;
using GrowthAI.Infrastructure.Repositories;

namespace GrowthAI.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IAppUserRepository _userRepository;
    private readonly JwtHelper _jwtHelper;

    // 绠€鏄揜efreshToken瀛樺偍锛堢敓浜х幆澧冨簲浣跨敤Redis锛?
    private static readonly Dictionary<string, (long UserId, long TenantId, DateTime ExpiresAt)> _refreshTokens = new();

    public AuthService(IAppUserRepository userRepository, JwtHelper jwtHelper)
    {
        _userRepository = userRepository;
        _jwtHelper = jwtHelper;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        // 閫氳繃鎵嬫満鍙锋煡鎵剧敤鎴凤紙闇€瑕佸厛纭畾绉熸埛锛孧VP闃舵绠€鍖栦负閬嶅巻锛?
        // 瀹為檯鐢熶骇涓簲鏈夌鎴蜂笂涓嬫枃鎴栫櫥褰曟椂閫夋嫨绉熸埛
        var user = await FindUserByPhoneAsync(request.Phone);
        if (user == null) return null;

        // 楠岃瘉瀵嗙爜
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        // 鐢熸垚Token
        var accessToken = _jwtHelper.GenerateAccessToken(user);
        var refreshToken = _jwtHelper.GenerateRefreshToken();

        // 瀛樺偍RefreshToken
        _refreshTokens[refreshToken] = (user.Id, user.TenantId, DateTime.UtcNow.AddDays(7));

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = 1440 * 60, // 24灏忔椂
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
        // 鐢熶骇鐜搴斿皢token鍔犲叆榛戝悕鍗曪紙Redis锛?
        _refreshTokens.Remove(token);
        return Task.CompletedTask;
    }

    private async Task<AppUser?> FindUserByPhoneAsync(string phone)
    {
        // MVP绠€鍖栵細閬嶅巻鎵€鏈夌鎴锋煡鎵剧敤鎴?
        // 鐢熶骇鐜搴斿湪鐧诲綍鏃朵紶閫扵enantId
        var users = await _userRepository.GetByTenantAsync(1); // 涓存椂纭紪鐮佺鎴?
        return users.FirstOrDefault(u => u.Phone == phone && u.Status == 1);
    }
}