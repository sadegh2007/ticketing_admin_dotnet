using System.Linq.Expressions;
using ERP.Shared.Users;

namespace ERP.Ticketing.HttpApi.Features.Users.Mappers;

public static class SimpleUserMapper
{
    public static Expression<Func<User, SimpleUserDto>> Mapper => user => new SimpleUserDto()
    {
        Id = user.Id,
        Name = user.Name,
        Family = user.Family
    };
}