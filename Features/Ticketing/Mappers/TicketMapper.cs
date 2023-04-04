using LinqKit;
using ERP.Shared.Ticketing;
using System.Linq.Expressions;
using ERP.Ticketing.HttpApi.Features.Categories.Mappers;
using ERP.Ticketing.HttpApi.Features.Users.Mappers;
using ERP.Ticketing.HttpApi.Features.Departments.Mappers;

namespace ERP.Ticketing.HttpApi.Features.Ticketing.Mappers;

public static class TicketMapper
{
    public static Expression<Func<Ticket, ListTicketDto>> ListMapper(Guid userId) => ticket => new ListTicketDto()
    {
        Id = ticket.Id,
        Title = ticket.Title,
        Number = ticket.Number,
        Department = ticket.Department != null ? DepartmentMapper.Mapper.Invoke(ticket.Department) : null,
        Creator = CreatorMapper.Mapper.Invoke(ticket.Creator),
        Status = TickStatusMapper.Mapper.Invoke(ticket.Status),
        ClosedBy = ticket.ClosedBy != null ? SimpleUserMapper.Mapper.Invoke(ticket.ClosedBy) : null,
        Categories = ticket.Categories.AsQueryable().AsExpandable().Where(x => x.CreatorId == userId).Select(CategoryMapper.TicketCategoryMapper).ToList(),
        IsClosed = ticket.IsClosed,
        LastCommentAt = ticket.LastCommentAt,
        UnreadCount = ticket.Comments.AsQueryable().Count(x => x.TicketCommentSeens.Count(s => s.CreatorId == userId) == 0),
        CreatedAt = ticket.CreatedAt
    };
    
    public static Expression<Func<Ticket, TicketDto>> Mapper => ticket => new TicketDto()
    {
        Id = ticket.Id,
        Title = ticket.Title,
        Number = ticket.Number,
        Department = ticket.Department != null ? DepartmentMapper.Mapper.Invoke(ticket.Department) : null,
        Creator = CreatorMapper.Mapper.Invoke(ticket.Creator),
        Status = TickStatusMapper.Mapper.Invoke(ticket.Status),
        ClosedBy = ticket.ClosedBy != null ? SimpleUserMapper.Mapper.Invoke(ticket.ClosedBy) : null,
        Comments = ticket.Comments.AsQueryable().AsExpandable().Select(TicketCommentMapper.Mapper).ToList(),
        IsClosed = ticket.IsClosed,
        LastCommentAt = ticket.LastCommentAt,
        CreatedAt = ticket.CreatedAt
    };
}