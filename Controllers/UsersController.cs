using ERP.Shared.Commons;
using ERP.Shared.Users;
using ERP.Ticketing.HttpApi.Commons.Attributes;
using ERP.Ticketing.HttpApi.Controllers.Common;
using ERP.Ticketing.HttpApi.Features.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Ticketing.HttpApi.Controllers;

public class UsersController: AuthApiControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("list",Name = "users_list")]
    // [HasPermission("UsersList")]
    public async Task<PagedResult<UserListDto>> ListOfUsers([FromBody] UserListFilter filters)
    {
        return await _userService.ListOfUsers(filters);
    }
    
    [HttpGet("{userId:guid}", Name = "get_user_by_id")]
    public async Task<IActionResult> GetById(Guid userId)
    {
        return Ok(await _userService.GetById(userId));
    }
    
    [HttpPost(Name = "create_user")]
    public async Task<IActionResult> Create([FromForm] UserCreateDto request)
    {
        return Ok(await _userService.Create(request));
    }
    
    [HttpPost("{userId:guid}", Name = "update_user")]
    public async Task<IActionResult> Update(Guid userId, [FromForm] UpdateUserDto request)
    {
        return Accepted(await _userService.Update(userId, request));
    }
    
    [HttpDelete("{userId:guid}", Name = "delete_user")]
    public async Task<IActionResult> Delete(Guid userId)
    {
        await _userService.Delete(userId);
        return NoContent();
    }
}