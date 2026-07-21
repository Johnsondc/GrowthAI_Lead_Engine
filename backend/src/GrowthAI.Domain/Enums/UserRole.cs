// ============================================
// 功能描述：认证与多租户模块 - 用户角色枚举
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Domain.Enums;

/// <summary>
/// 用户角色枚举
/// </summary>
public enum UserRole
{
    /// <summary>管理员 - 全部权限</summary>
    Admin = 0,
    /// <summary>老板 - 只读查看</summary>
    Owner = 1,
    /// <summary>运营 - 内容/引流/获客</summary>
    Operator = 2,
    /// <summary>销售 - 客户跟进</summary>
    Sales = 3
}
