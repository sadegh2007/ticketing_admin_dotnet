using System.Linq.Expressions;
using ERP.Shared.Users;

namespace ERP.Ticketing.HttpApi.Features.Users.Mappers;

public static class UserMapper
{
    public static Expression<Func<User, UserListDto>> ListMapper => user => new UserListDto()
    {
        Id = user.Id,
        Name = user.Name,
        Family = user.Family,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        UserName = user.UserName,
        Picture = user.Picture,
        Roles = user.UserRoles.AsQueryable().Select(x => x.Role).Select(RoleMapper.ListMapper).ToList()
    };
    
    public static Expression<Func<User, UserDto>> Mapper => user => new UserDto()
    {
        Id = user.Id,
        Name = user.Name,
        Family = user.Family,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        UserName = user.UserName,
        Picture = user.Picture
    };
}