using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Tenants;
using Microsoft.AspNetCore.Identity;

namespace ERP.Ticketing.HttpApi.Features.Users;

public class User: IdentityUser<Guid>, IModel, ICreateTime, IUpdateTime, ISoftDelete
{
    private User()
    {
    }

    public User(string mobile)
    {
        PhoneNumber = mobile;
        UserName = mobile;
    }
    
    public string? Picture { get; set; }
    public string? Name { get; set; }
    public string? Family { get; set; }
    public string? FullName => Name != null ? (Name + (Family != null ? (" " + Family) : "")) : null;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public User? DeletedBy { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<TenantUser> TenantUsers { get; set; } = new List<TenantUser>();
}