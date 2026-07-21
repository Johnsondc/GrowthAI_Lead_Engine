// ============================================
// 功能描述：企业管理模块 - 用户管理控制器
// Sprint: 5 (M2 企业管理)
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
// ============================================

using GrowthAI.Application.Authorization;
using GrowthAI.Application.Enterprise;
using GrowthAI.Application.Enterprise.Dto;
using GrowthAI.Application.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/settings/users")]
[Authorize(Policy = RolePolicies.AdminOnly)]
public class UsersController : ControllerBase
{
    private readonly IAppUserService _userService;

    public UsersController(IAppUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 获取员工列表
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var tenantId = HttpContext.GetTenantId();
        var users = await _userService.GetUsersAsync(tenantId);
        return Ok(users);
    }

    /// <summary>
    /// 获取单个员工信息
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetUser(long id)
    {
        var tenantId = HttpContext.GetTenantId();
        var user = await _userService.GetUserByIdAsync(tenantId, id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    /// <summary>
    /// 创建员工
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateAppUserRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var user = await _userService.CreateUserAsync(tenantId, request);
        if (user == null)
            return BadRequest(new { message = "手机号已存在" });
        return Ok(user);
    }

    /// <summary>
    /// 更新员工信息
    /// </summary>
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateUser(long id, [FromBody] UpdateAppUserRequest request)
    {
        var tenantId = HttpContext.GetTenantId();
        var user = await _userService.UpdateUserAsync(tenantId, id, request);
        if (user == null)
            return BadRequest(new { message = "用户不存在或手机号已存在" });
        return Ok(user);
    }

    /// <summary>
    /// 停用员工（软删除）
    /// </summary>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        var tenantId = HttpContext.GetTenantId();
        var success = await _userService.DeleteUserAsync(tenantId, id);
        if (!success) return NotFound();
        return Ok(new { message = "已停用" });
    }
}
