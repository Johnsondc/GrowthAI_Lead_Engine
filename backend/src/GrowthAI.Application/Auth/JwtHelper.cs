// ============================================
// 功能描述：认证与多租户模块 - JWT工具类
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GrowthAI.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace GrowthAI.Application.Auth;

public class JwtHelper
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiresMinutes;

    public JwtHelper(string secret, string issuer, string audience, int expiresMinutes = 1440)
    {
        _secret = secret;
        _issuer = issuer;
        _audience = audience;
        _expiresMinutes = expiresMinutes;
    }

    public string GenerateAccessToken(AppUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("TenantId", user.TenantId.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.MobilePhone, user.Phone)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expiresMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray().Concat(Guid.NewGuid().ToByteArray()).ToArray());
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var handler = new JwtSecurityTokenHandler();
        try
        {
            return handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ValidateLifetime = true,
                IssuerSigningKey = key
            }, out _);
        }
        catch
        {
            return null;
        }
    }
}
