using System.Linq.Expressions;
using ERP.Shared.Roles;

namespace ERP.Ticketing.HttpApi.Features.Users.Mappers;

public static class RoleMapper
{
    public static Expression<Func<Role, RoleDto>> ListMapper => role => new RoleDto()
    {
        Id = role.Id,
        Name = role.Name!,
        Title = role.Title
    };
    
    public static Expression<Func<Role, RoleDto>> Mapper => role => new RoleDto()
    {
        Id = role.Id,
        Name = role.Name!,
        Title = role.Title,
        Permissions = role.Permissions.AsQueryable().Select(PermissionMapper.Mapper).ToList()
    };
}