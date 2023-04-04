using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketCommentSeen: IModel, ICreator, ICreateTime
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
    public TicketComment Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}