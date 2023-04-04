using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Data.Common.Helpers;

public interface ICreator<TKey>
{
    public TKey CreatorId { get; set; }
    public User Creator { get; set; }
}

public interface ICreator : ICreator<Guid> {}