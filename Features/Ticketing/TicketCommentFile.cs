using ERP.Ticketing.HttpApi.Features.Users;
using ERP.Ticketing.HttpApi.Data.Common.Helpers;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketCommentFile: IModel, ICreator, ICreateTime, IUpdateTime
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
    public Guid TicketCommentId { get; set; }
    public TicketComment TicketComment { get; set; }
    public string Path { get; set; }
    public string FileName { get; set; }
    public string? Extension { get; set; }
    public long? Size { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}