using System.Linq.Expressions;
using ERP.Shared.Users;

namespace ERP.Ticketing.HttpApi.Features.Users.Mappers;

public static class CreatorMapper
{
    public static Expression<Func<User, CreatorDto>> Mapper => user => new CreatorDto()
    {
        Id = user.Id,
        Name = user.Name,
        Family = user.Family,
        Email = user.Email,
        Picture = user.Picture
    };
}