using System.Linq.Expressions;
using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Features.Users.Mappers;
using LinqKit;

namespace ERP.Ticketing.HttpApi.Features.Ticketing.Mappers;

public static class TicketCommentMapper
{
    public static Expression<Func<TicketComment, TicketCommentDto>> Mapper => comment => new TicketCommentDto()
    {
        Id = comment.Id,
        TicketId = comment.Ticket.Id,
        Message = comment.Message,
        Type = comment.Type,
        Creator = CreatorMapper.Mapper.Invoke(comment.Creator),
        // Replay = comment.Replay != null ? ReplyMapper.Invoke(comment.Replay) : null,
        // Files = comment.Files.AsQueryable().AsExpandable().Select(FileMapper).ToList(),
        IsEdited = comment.IsEdited,
        CreatedAt = comment.CreatedAt,
        UpdatedAt = comment.UpdatedAt
    };

    public static Expression<Func<TicketComment, TicketCommentDto>> ReplyMapper => comment => new TicketCommentDto()
    {
        Id = comment.Id,
        Message = comment.Message,
        CreatedAt = comment.CreatedAt,
        Creator = CreatorMapper.Mapper.Invoke(comment.Creator),
        Type = comment.Type,
    };

    public static Expression<Func<TicketCommentFile, TicketCommentFileDto>> FileMapper => file => new TicketCommentFileDto()
    {
        Id = file.Id,
        Path = file.Path,
        FileName = file.FileName,
        TicketCommentId = file.TicketCommentId
    };
}