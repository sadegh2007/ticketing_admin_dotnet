using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public enum TicketUserHistoryType
{
    ADD = 0,
    DELETE = 1,
    LEFT = 2
}

public class TicketUserHistory: IModel, ICreator, ICreateTime
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public TicketUserHistoryType Type { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
    public DateTime CreatedAt { get; set; }
}