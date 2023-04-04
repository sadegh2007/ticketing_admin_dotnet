using System.Linq.Expressions;
using ERP.Shared.Roles;

namespace ERP.Ticketing.HttpApi.Features.Users.Mappers;

public class PermissionMapper
{
    public static Expression<Func<Permission, PermissionDto>> Mapper = permission => new PermissionDto()
    {
        Id = permission.Id,
        Route = permission.Route,
        Name = permission.Name,
        Title = permission.Title,
        CreatedAt = permission.CreatedAt
    };
}