using System.Linq.Expressions;
using ERP.Shared.Ticketing;

namespace ERP.Ticketing.HttpApi.Features.Ticketing.Mappers;

public static class TickStatusMapper
{
    public static Expression<Func<TicketStatus, TicketStatusDto>> Mapper => status => new TicketStatusDto()
    {
        Id = status.Id,
        Name = status.Name,
        Title = status.Title,
    };
}