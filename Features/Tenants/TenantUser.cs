using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Features.Tenants;

public class TenantUser: IModel, ITenantable
{
    public Guid Id { get; set; }
    public string TenantId { get; set; }
    public Tenant Tenant { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}