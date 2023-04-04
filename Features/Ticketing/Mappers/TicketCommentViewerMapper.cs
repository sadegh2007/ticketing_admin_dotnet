using LinqKit;
using ERP.Shared.Ticketing;
using System.Linq.Expressions;
using ERP.Ticketing.HttpApi.Features.Users.Mappers;

namespace ERP.Ticketing.HttpApi.Features.Ticketing.Mappers;

public static class TicketCommentViewerMapper
{
    public static Expression<Func<TicketCommentSeen, TicketCommentSeenDto>> Mapper = seen =>
        new TicketCommentSeenDto()
        {
            Id = seen.Id,
            Creator = CreatorMapper.Mapper.Invoke(seen.Creator),
            CreatedAt = seen.CreatedAt
        };
}