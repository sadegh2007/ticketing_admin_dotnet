using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Departments;
using ERP.Ticketing.HttpApi.Features.Ticketing;
using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Features.Tenants;

public class Tenant: ICreator, ICreateTime, IUpdateTime, ISoftDelete
{
    public Tenant()
    {
        // Users = new List<User>();
        Tickets = new List<Ticket>();
        Departments = new List<Department>();
    }
    
    public string Id { get; set; }
    public string Title { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
    // public ICollection<User> Users { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
    public ICollection<Department> Departments { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public User? DeletedBy { get; set; }
}