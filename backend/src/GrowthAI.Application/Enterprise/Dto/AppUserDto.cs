// ============================================
// 功能描述：企业管理模块 - 用户管理DTO
// Sprint: 5 (M2 企业管理)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

namespace GrowthAI.Application.Enterprise.Dto;

/// <summary>
/// 系统用户DTO
/// </summary>
public class AppUserDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 创建用户请求
/// </summary>
public class CreateAppUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Operator";
}

/// <summary>
/// 更新用户请求
/// </summary>
public class UpdateAppUserRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Role { get; set; }
    public int? Status { get; set; }
}
