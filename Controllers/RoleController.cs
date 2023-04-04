using ERP.Shared.Commons;
using ERP.Shared.Roles;
using ERP.Ticketing.HttpApi.Controllers.Common;
using ERP.Ticketing.HttpApi.Features.Users;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Ticketing.HttpApi.Controllers;

[Route("api/users/roles")]
public class RoleController: AuthApiControllerBase
{
    private readonly RoleService _roleService;

    public RoleController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("list", Name = "roles_list")]
    public async Task<PagedResult<RoleDto>> List(RoleListFilter filters)
    {
        return await _roleService.ListOfRoles(filters);
    }
    
    [HttpGet("{roleId:guid}", Name = "get_role_by_id")]
    public async Task<RoleDto> GetById(Guid roleId)
    {
        return await _roleService.GetById(roleId);
    }
    
    [HttpPost(Name = "create_role")]
    public async Task<RoleDto> Create(CreateRoleDto request)
    {
        return await _roleService.Create(request);
    }
    
    [HttpPut("{roleId:guid}", Name = "update_role")]
    public async Task<IActionResult> Update(Guid roleId, CreateRoleDto request)
    {
        var result = await _roleService.Update(roleId, request);
        return Accepted(result);
    }
    
    [HttpDelete("{roleId:guid}", Name = "delete_role")]
    public async Task<IActionResult> Delete(Guid roleId)
    {
        await _roleService.Delete(roleId);
        return Ok();
    }
}