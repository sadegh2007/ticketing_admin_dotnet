using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Data.Common.Helpers;

public interface ISoftDelete
{
    public DateTime? DeletedAt { get; set; }
    public User? DeletedBy { get; set; }
}