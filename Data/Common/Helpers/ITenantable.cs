using ERP.Ticketing.HttpApi.Features.Tenants;

namespace ERP.Ticketing.HttpApi.Data.Common.Helpers;

public interface ITenantable
{
    public string TenantId { get; set; }
    public Tenant Tenant { get; set; }
}