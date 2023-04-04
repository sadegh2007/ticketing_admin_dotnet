using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Tenants;

namespace ERP.Ticketing.HttpApi.Features.Users;

public class UserRole: IModel, ICreateTime, ITenantable
{
    public Guid Id { get; set; }
    
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public string TenantId { get; set; }
    public Tenant Tenant { get; set; }
    
    public DateTime CreatedAt { get; set; }
}