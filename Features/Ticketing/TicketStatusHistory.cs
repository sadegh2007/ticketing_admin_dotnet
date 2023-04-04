using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketStatusHistory: IModel, ICreator, ICreateTime
{
    public Guid Id { get; set; }
    public TicketStatus Status { get; set; }
    public Ticket Ticket { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
    public DateTime CreatedAt { get; set; }
}