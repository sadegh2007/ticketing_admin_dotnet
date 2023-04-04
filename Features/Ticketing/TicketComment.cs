using System.Collections.ObjectModel;
using ERP.Ticketing.HttpApi.Features.Users;
using ERP.Ticketing.HttpApi.Data.Common.Helpers;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketComment: IModel, ICreator, ICreateTime, IUpdateTime, ISoftDelete, IShamsiCreatedAt
{
    public TicketComment()
    {
        TicketCommentSeens = new Collection<TicketCommentSeen>();
        Files = new List<TicketCommentFile>();
    }
    
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
    public Ticket Ticket { get; set; }
    public string Message { get; set; }
    public bool IsEdited { get; set; } = false;
    public string Type { get; set; } = "comment";

    public Guid? ReplayId { get; set; }
    public TicketComment? Replay { get; set; }
    public ICollection<TicketCommentSeen> TicketCommentSeens { get; set; }
    public ICollection<TicketCommentFile> Files { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public User? DeletedBy { get; set; }
    public string ShamsiCreatedAt { get; set; }
}