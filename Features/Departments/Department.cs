using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Tenants;
using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Features.Departments;

public class Department: IModel, ICreator, ICreateTime, IUpdateTime, IShamsiCreatedAt, ITenantable
{
    public Department()
    {
        DepartmentUsers = new List<DepartmentUser>();
    }
    
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string ShamsiCreatedAt { get; set; }
    public string TenantId { get; set; }
    public Tenant Tenant { get; set; }
    public ICollection<DepartmentUser> DepartmentUsers { get; set; }
}