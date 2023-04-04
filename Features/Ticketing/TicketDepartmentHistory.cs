using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Departments;
using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketDepartmentHistory: IModel, ICreator, ICreateTime
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; }
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
    public DateTime CreatedAt { get; set; }
}