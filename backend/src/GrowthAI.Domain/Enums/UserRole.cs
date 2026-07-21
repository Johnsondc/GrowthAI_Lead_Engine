// ============================================
// 鍔熻兘鎻忚堪锛氳璇佷笌澶氱鎴锋ā鍧?- 鐢ㄦ埛瑙掕壊鏋氫妇
// 鐢熸垚锛歈oder by 搴勫洯
// 鐢熸垚鏃ユ湡锛?026-07-21
// ============================================

namespace GrowthAI.Domain.Enums;

/// <summary>
/// 鐢ㄦ埛瑙掕壊鏋氫妇
/// </summary>
public enum UserRole
{
    /// <summary>绠＄悊鍛?- 鍏ㄩ儴鏉冮檺</summary>
    Admin = 0,
    /// <summary>鑰佹澘 - 鍙鏌ョ湅</summary>
    Owner = 1,
    /// <summary>杩愯惀 - 鍐呭/寮曟祦/鑾峰</summary>
    Operator = 2,
    /// <summary>閿€鍞?- 瀹㈡埛璺熻繘</summary>
    Sales = 3
}