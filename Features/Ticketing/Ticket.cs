using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Departments;
using ERP.Ticketing.HttpApi.Features.Tenants;
using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class Ticket: IModel, ICreator, ICreateTime, IUpdateTime, ISoftDelete, IShamsiCreatedAt, ITenantable
{
    public Guid Id { get; set; }
    public long Number { get; set; } = 1;
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
    public string Title { get; set; }
    public TicketStatus Status { get; set; }
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Shared.Ticketing.TicketPriority Priority { get; set; } = Shared.Ticketing.TicketPriority.Low;
    public DateTime LastCommentAt { get; set; }
    public bool IsClosed { get; set; } = false;
    public User? ClosedBy { get; set; }
    public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
    public ICollection<TicketUser> Users { get; set; } = new List<TicketUser>();
    public ICollection<TicketUserHistory> UserHistories { get; set; } = new List<TicketUserHistory>();
    public ICollection<TicketDepartmentHistory> DepartmentHistories { get; set; } = new List<TicketDepartmentHistory>();
    public ICollection<TicketCategory> Categories { get; set; } = new List<TicketCategory>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public User? DeletedBy { get; set; }
    public string ShamsiCreatedAt { get; set; }
    public string TenantId { get; set; }
    public Tenant Tenant { get; set; }
}