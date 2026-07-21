// ============================================
// 鍔熻兘鎻忚堪锛氳璇佷笌澶氱鎴锋ā鍧?- 鍒锋柊Token璇锋眰DTO
// 鐢熸垚锛歈oder by 搴勫洯
// 鐢熸垚鏃ユ湡锛?026-07-21
// ============================================

namespace GrowthAI.Application.Auth.Dto;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}